using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Board.Script;
using BoomLib.Tools;
using BoomLib.UI.Scripts;
using Cards.Scripts;
using CardSlot.Script;
using CombatLoop.Battles.Data;
using CombatLoop.EnergyBar;
using Localization;
using Run_Loop;
using UnityEngine;
using UnityEngine.Events;

namespace CombatLoop
{
    public class CombatLoop : MonoBehaviour
    {
        [SerializeField] private EnemyHandController enemyHandController;
        [SerializeField] private PlayerHandController playerHandController;
        [SerializeField] public CardContainer playerBoard;
        
        [Space]
        [SerializeField] private Button endPreparationButton;
        [SerializeField] private Button endPlayerTurnButton;

        [Space] 
        [SerializeField] private TurnEndAnimation turnEndAnimation;

        public static UnityEvent OnPlayerDrawHand = new UnityEvent();
        public static UnityEvent OnPlayerPlayAtLeastOneCard = new UnityEvent();
        public static UnityEvent OnPlayerPlayStartFirstTurn = new UnityEvent();
        
        public enum TurnType
        {
            Preparation,
            SetupOver,
            Player,
            Enemy
        }

        public TurnType currentTurn { get; private set; }
        public int turnCount { get; private set; }

        public static CombatLoop instance;

        private void Awake()
        {
            instance = this;
        }

        private IEnumerator Start()
        {
            turnCount = 0;
            currentTurn = TurnType.Preparation;
            yield return SetupButtons();
            yield return FightIntro();
            yield return PlaceEnemyCards(RunLoop.instance.SelectBattle());
            yield return DrawCards();
            OnPlayerDrawHand?.Invoke();
            yield return WaitForAtLeastOneCardOnPlayerBoard();
            OnPlayerPlayAtLeastOneCard?.Invoke();
            yield return ActivateEndPreparationButton();
            yield return PlayHand();
            yield return DeactivateEndPreparationButton();
            yield return TransformBattleground();
            yield return DeactivatePlayerHand();
            yield return ActivatePlayerEnergyDisplay();
            
            currentTurn = TurnType.Player;
            turnCount = 1;
            yield return StartTurn(TurnType.Player);
            OnPlayerPlayStartFirstTurn?.Invoke();
            
            while (IsMatchOver() == false)
            {
                yield return ActivatePlayerEndTurnButton();
                yield return DoPlayerTurn();
                yield return DeactivatePlayerEndTurnButton();
                yield return EndTurn(TurnType.Player);
                
                yield return StartTurn(TurnType.Enemy);
                yield return DoEnemyTurn();
                yield return EndTurn(TurnType.Enemy);
                
                yield return StartTurn(TurnType.Player);
                yield return RefreshPlayerEnergyCount();
                turnCount += 1;
            }
        }

        private void OnEnable()
        {
            ActionSystem.AttachPerformer<StartTurnGa>(StartTurnPerformer);
            ActionSystem.AttachPerformer<EndTurnGA>(EndTurnPerformer);
            ActionSystem.AttachPerformer<SpawnCardGA>(SpawnCardPerformer);
        }

        private void OnDisable()
        {
            ActionSystem.DetachPerformer<StartTurnGa>();
            ActionSystem.DetachPerformer<EndTurnGA>();
            ActionSystem.DetachPerformer<SpawnCardGA>();
        }
        
        private IEnumerator SpawnCardPerformer(SpawnCardGA spawnCardGa)
        {
            if (spawnCardGa.cardData.isEnemy)
                spawnCardGa.spawnedCard = enemyHandController.SpawnEnemy(spawnCardGa.cardData);
            else if (spawnCardGa.isToken)
                spawnCardGa.spawnedCard = playerHandController.SpawnToken(spawnCardGa);
            else    
                spawnCardGa.spawnedCard = playerHandController.SpawnCard(new DeckCard(spawnCardGa.cardData), playerBoard);
            
            if (spawnCardGa.startingHealth > 0)
                spawnCardGa.spawnedCard.cardHealth.SetHealth(spawnCardGa.startingHealth);
            
            if (spawnCardGa.deckCard != null)
                spawnCardGa.spawnedCard.SetDeckCard(spawnCardGa.deckCard);
            
            yield return new WaitForSeconds(0.2f);
        }

        private IEnumerator SetupButtons()
        {
            endPreparationButton.SetText(LocalizationSystem.instance.GetCombatString("send_it_button"));
            endPreparationButton.gameObject.SetActive(false);
            
            endPlayerTurnButton.SetText(LocalizationSystem.instance.GetCombatString("end_turn_button"));
            endPlayerTurnButton.gameObject.SetActive(false);
            yield break;
        }
        
        private IEnumerator FightIntro()
        {
            yield return new WaitForSeconds(1.0f);
        }
        
        private IEnumerator PlaceEnemyCards(BattleData battle)
        {
            yield return enemyHandController.SetupBattle(battle);
        }
        
        private IEnumerator DrawCards()
        {
            yield return playerHandController.DrawHand();
        }
        
        private IEnumerator WaitForAtLeastOneCardOnPlayerBoard()
        {
            yield return new WaitUntil(() => playerBoard.Slots.Count > 0);
        }
        
        private IEnumerator ActivateEndPreparationButton()
        {
            endPreparationButton.gameObject.SetActive(true);
            yield break;
        }
        
        private IEnumerator DeactivateEndPreparationButton()
        {
            endPreparationButton.gameObject.SetActive(false);
            yield break;
        }
        
        private IEnumerator ActivatePlayerEndTurnButton()
        {
            endPlayerTurnButton.gameObject.SetActive(true);
            yield break;
        }
        
        private IEnumerator DeactivatePlayerEndTurnButton()
        {
            endPlayerTurnButton.gameObject.SetActive(false);
            yield break;
        }
        
        private IEnumerator PlayHand()
        {
            yield return new WaitWhile(() => currentTurn == TurnType.Preparation);
        }

        public void OnEndSetupPhase()
        {
            currentTurn = TurnType.SetupOver;
        }
        
        private IEnumerator TransformBattleground()
        {
            yield return new WaitForSeconds(0.5f);
        }
        
        private IEnumerator DeactivatePlayerHand()
        {
            playerHandController.DeactivateHand();
            yield return null;
        }
        
        private IEnumerator ActivatePlayerEnergyDisplay()
        {
            EnergyController.instance.Initialize();
            yield return null;
        }

        private IEnumerator RefreshPlayerEnergyCount()
        {
            EnergyController.instance.RefillEnergy();
            yield return null;
        }

        private IEnumerator EndTurn(TurnType turnType)
        {
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                
            Debug.Log($"End Turn : current : {turnType} / target : {turnType.Opposite()}");
            EndTurnGA endTurnGa = new EndTurnGA(turnType);
            ActionSystem.instance.Perform(endTurnGa);
            
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
        }
        
        private IEnumerator EndTurnPerformer(EndTurnGA endTurnGA)
        {
            currentTurn = endTurnGA.ending.Opposite();
            yield return turnEndAnimation.PlayAnimation(currentTurn);
        }
        
        private IEnumerator StartTurn(TurnType turnType)
        {
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                
            Debug.Log($"Start Turn : current : {turnType}");
            StartTurnGa startTurnGa = new StartTurnGa(turnType);
            ActionSystem.instance.Perform(startTurnGa);
            
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
        }
        
        private IEnumerator StartTurnPerformer(StartTurnGa startTurnGa)
        {
            currentTurn = startTurnGa.starting;
            yield break;
        }

        private IEnumerator DoEnemyTurn()
        {
            yield return enemyHandController.PlayTurn();
        }

        private bool isPlayerPlaying;
        private IEnumerator DoPlayerTurn()
        {
            isPlayerPlaying = true;
            yield return new WaitWhile(() => isPlayerPlaying);
            yield return new WaitWhile(() => ActionSystem.instance.isLocked);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
        }
        
        public void OnEndPlayerTurn()
        {
            isPlayerPlaying = false;
        }
        
        public bool IsMatchOver()
        {
            bool isPlayerDead = playerBoard.Slots.Count == 0;
            
            return currentTurn != TurnType.SetupOver && currentTurn != TurnType.Preparation && (isPlayerDead || IsEnemyDead());
        }

        public bool IsEnemyDead()
        {
            if (enemyHandController.container.Slots.Count == 0)
                return true;

            foreach (Slot slot in enemyHandController.container.Slots)
            {
                if (slot.CurrentCard.cardController.cardData.isMainBoss)
                    return false;
            }

            return true;
        }

        public bool HasPlayerWon()
        {
            return IsEnemyDead();
        }

        public void StoreCardsHealth()
        {
            foreach (Slot slot in playerBoard.Slots)
            {
                CardController card = slot.CurrentCard.cardController;
                
                if (!card.cardHealth.IsDead)
                    PlayerDeck.instance.UpdateCardHealthPoints(card.deckCard, card.cardHealth.currentHealth);
            }
        }
    }
}
