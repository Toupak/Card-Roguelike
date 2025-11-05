using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Battles.Data;
using Board.Script;
using Cards.Scripts;
using Cards.Tween_Animations;
using CardSlot.Script;
using EnemyAttack;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CombatLoop
{
    public class EnemyHandController : MonoBehaviour
    {
        [SerializeField] private CardContainer enemyBoardContainer;
        [SerializeField] private CardMovement cardMovementPrefab;
        [SerializeField] private List<BattleData> battles;
        
        public CardContainer container => enemyBoardContainer;

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

                CardController card = slots[i].CurrentCard.cardController;

                if (card.cardStatus.IsStatusApplied(StatusType.Stun))
                {
                    Debug.Log("Enemy is Stun, skip its turn");
                    yield return CardTween.PlayCardIsStun(card);
                    continue;
                }

                EnemyPerformsActionGa enemyPerformsActionGa = new EnemyPerformsActionGa(card);
                ActionSystem.instance.Perform(enemyPerformsActionGa);
                
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                
                if (card != null && !card.cardHealth.IsDead)
                    yield return card.enemyCardController!.ExecuteIntention();
                
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            }
        }

        private void ComputeAndDisplayEachCardsNextIntention()
        {
            foreach (Slot slot in enemyBoardContainer.Slots)
            {
                EnemyCardController card = slot.CurrentCard.cardController.enemyCardController;
                card!.ComputeNextIntention();
                card.DisplayNextIntention();
            }
        }

        public IEnumerator SetupBattle()
        {
            int randomIndex = Random.Range(0, battles.Count);

            foreach (CardData data in battles[randomIndex].enemyList)
            {
                SpawnEnemy(data);
                yield return new WaitForSeconds(0.15f);
            }
        }

        public void SpawnEnemy(CardData enemyData)
        {
            CardMovement newCard = Instantiate(cardMovementPrefab);
            enemyBoardContainer.ReceiveCard(newCard);

            CardController controller = CardsVisualManager.instance.SpawnNewCardVisuals(newCard, enemyData);
            newCard.SetCardController(controller);
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
