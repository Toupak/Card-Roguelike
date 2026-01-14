using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Board.Script;
using Cards.Scripts;
using Cards.Tween_Animations;
using CardSlot.Script;
using CombatLoop.Battles.Data;
using EnemyAttack;
using Run_Loop;
using UnityEngine;

namespace CombatLoop
{
    public class EnemyHandController : MonoBehaviour
    {
        public static EnemyHandController instance;

        [SerializeField] private CardContainer enemyBoardContainer;
        [SerializeField] private CardMovement cardMovementPrefab;
        
        public CardContainer container => enemyBoardContainer;

        private void Awake()
        {
            if (instance != null && instance != this)
                Destroy(this);
            else
                instance = this;
        }

        public IEnumerator PlayTurn()
        {
            yield return ExecuteEachCardBehaviour();
            ComputeAndDisplayEachCardsNextIntention();
        }

        private IEnumerator ExecuteEachCardBehaviour()
        {
            List<Slot> slots = enemyBoardContainer.Slots;

            for (int i = slots.Count - 1; i >= 0; i--)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                if (slots[i] == null || slots[i].CurrentCard == null || slots[i].CurrentCard.cardController == null)
                    continue;
                
                CardController card = slots[i].CurrentCard.cardController;

                if (card.cardStatus.IsStatusApplied(StatusType.Stun))
                {
                    Debug.Log("Enemy is Stun, skip its turn");
                    yield return CardTween.PlayCardIsStun(card);
                    
                    if (card.enemyCardController != null)
                        card.enemyCardController.SkipIntention();
                    
                    continue;
                }

                if (card != null && card.enemyCardController != null && !card.enemyCardController.isWaiting)
                {
                    EnemyPerformsActionGa enemyPerformsActionGa = new EnemyPerformsActionGa(card);
                    ActionSystem.instance.Perform(enemyPerformsActionGa);
                }
                
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                
                if (card != null && card.enemyCardController != null && !card.cardHealth.IsDead)
                    yield return card.enemyCardController!.ExecuteIntention();
                
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            }
        }

        private void ComputeAndDisplayEachCardsNextIntention()
        {
            foreach (Slot slot in enemyBoardContainer.Slots)
            {
                EnemyCardController card = slot.CurrentCard.cardController.enemyCardController;

                if (card != null)
                {
                    card!.ComputeNextIntention();
                    card.DisplayNextIntention();
                }
            }
        }

        public IEnumerator SetupBattle(BattleData battle)
        {
            enemyBoardContainer.ResetContainer();
            foreach (CardData data in battle.enemyList)
            {
                SpawnEnemy(data);
                yield return new WaitForSeconds(0.15f);
            }
        }

        public CardController SpawnEnemy(CardData enemyData)
        {
            CardMovement newCard = Instantiate(cardMovementPrefab);
            enemyBoardContainer.ReceiveCard(newCard, enemyData.preferredPosition);

            CardController controller = CardsVisualManager.instance.SpawnNewCardVisuals(newCard, new DeckCard(enemyData));
            newCard.SetCardController(controller);

            return controller;
        }
        
        public void OnEnable()
        {
            ActionSystem.AttachPerformer<EnemyPerformsActionGa>(EnemyPerformsActionPerformer);
        }

        private void OnDisable()
        {
            ActionSystem.DetachPerformer<EnemyPerformsActionGa>();
        }
        
        private IEnumerator EnemyPerformsActionPerformer(EnemyPerformsActionGa stunGa)
        {
            yield break;
        }
    }
}
