using System.Collections;
using ActionReaction;
using ActionReaction.Game_Actions;
using Board.Script;
using BoomLib.Tools;
using Cards.Scripts;
using CardSlot.Script;
using CombatLoop.EnergyBar;
using Run_Loop;
using UnityEngine;

namespace CombatLoop
{
    public class CombatLoop : MonoBehaviour
    {
        [SerializeField] private EnemyHandController enemyHandController;
        [SerializeField] private PlayerHandController playerHandController;
        [SerializeField] private CardContainer playerBoard;
        [SerializeField] private Animator canvasAnimator;
        
        [SerializeField] private GameObject endPreparationButton;
        [SerializeField] private GameObject endPlayerTurnButton;
        
        public enum TurnType
        {
            Preparation,
            SetupOver,
            Player,
            Enemy
        }

        public TurnType currentTurn { get; private set; }

        public static CombatLoop instance;

        private void Awake()
        {
            instance = this;
        }

        private IEnumerator Start()
        {
            endPreparationButton.SetActive(false);
            endPlayerTurnButton.SetActive(false);
            
            currentTurn = TurnType.Preparation;
            yield return FightIntro();
            yield return PlaceEnemyCards();
            yield return DrawCards();
            yield return WaitForAtLeastOneCardOnPlayerBoard();
            yield return ActivateEndPreparationButton();
            yield return PlayHand();
            yield return DeactivateEndPreparationButton();
            yield return TransformBattleground();
            yield return ActivatePlayerEnergyDisplay();
            
            currentTurn = TurnType.Player;
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
            }
        }
        
        private void OnEnable()
        {
            ActionSystem.AttachPerformer<StartTurnGa>(StartTurnPerformer);
            ActionSystem.AttachPerformer<EndTurnGA>(EndTurnPerformer);
        }

        private void OnDisable()
        {
            ActionSystem.DetachPerformer<StartTurnGa>();
            ActionSystem.DetachPerformer<EndTurnGA>();
        }

        private IEnumerator FightIntro()
        {
            yield return new WaitForSeconds(1.0f);
        }
        
        private IEnumerator PlaceEnemyCards()
        {
            yield return enemyHandController.SetupBattle();
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
            endPreparationButton.SetActive(true);
            yield break;
        }
        
        private IEnumerator DeactivateEndPreparationButton()
        {
            endPreparationButton.SetActive(false);
            yield break;
        }
        
        private IEnumerator ActivatePlayerEndTurnButton()
        {
            endPlayerTurnButton.SetActive(true);
            yield break;
        }
        
        private IEnumerator DeactivatePlayerEndTurnButton()
        {
            endPlayerTurnButton.SetActive(false);
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
            canvasAnimator.Play("GoToBattle");
            yield return new WaitForSeconds(0.5f);
        }
        
        private IEnumerator ActivatePlayerEnergyDisplay()
        {
            EnergyController.instance.Initialize();
            yield return null;
        }

        private IEnumerator RefreshPlayerEnergyCount()
        {
            EnergyController.instance.RefreshOnTurnStart();
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
            yield break;
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
        }
        
        public void OnEndPlayerTurn()
        {
            isPlayerPlaying = false;
        }
        
        public bool IsMatchOver()
        {
            return currentTurn != TurnType.SetupOver && currentTurn != TurnType.Preparation && (playerBoard.Slots.Count == 0 || enemyHandController.container.Slots.Count == 0);
        }

        public bool HasPlayerWon()
        {
            return playerBoard.Slots.Count > enemyHandController.container.Slots.Count;
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
