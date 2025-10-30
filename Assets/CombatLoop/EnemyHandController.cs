using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Board.Script;
using Cards.Scripts;
using CardSlot.Script;
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

        internal IEnumerator PlayTurn()
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
            }
        }
        
        private void Update()
        {
            CheckDrawCard();
        }
        
        private void CheckDrawCard()
        {
            if (Keyboard.current.eKey.wasPressedThisFrame && !enemyBoardContainer.IsFull)
                DrawCard();
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
