using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Board.Script;
using Cards.Scripts;
using CardSlot.Script;
using EnemyAttack;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace CombatLoop
{
    public class EnemyHandController : MonoBehaviour
    {
        [SerializeField] private CardContainer enemyBoardContainer;
        [SerializeField] private CardMovement cardMovementPrefab;
        [SerializeField] private List<CardData> cardData;
        
        public CardContainer container => enemyBoardContainer;

        public IEnumerator PlayTurn()
        {
            yield return ExecuteEachCardBehaviour();
            ComputeAndDisplayEachCardsNextIntention();
        }

        private IEnumerator ExecuteEachCardBehaviour()
        {
            foreach (Slot slot in enemyBoardContainer.Slots)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                
                CardController card = slot.CurrentCard.cardController;

                if (card.cardStatus.IsStun)
                {
                    Debug.Log("Enemy is Stun, skip its turn");
                    yield return new WaitForSeconds(0.5f);
                    continue;
                }

                EnemyPerformsActionGa enemyPerformsActionGa = new EnemyPerformsActionGa(card);
                ActionSystem.instance.Perform(enemyPerformsActionGa);
                
                yield return card.enemyCardController!.ExecuteIntention();
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

        public void DrawCard()
        {
            CardMovement newCard = Instantiate(cardMovementPrefab);
            enemyBoardContainer.ReceiveCard(newCard);

            int randomIndex = Random.Range(0, cardData.Count);
            CardController controller = CardsVisualManager.instance.SpawnNewCardVisuals(newCard, cardData[randomIndex]);
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
