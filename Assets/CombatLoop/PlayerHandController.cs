using System.Collections;
using System.Collections.Generic;
using Board.Script;
using Cards.Scripts;
using Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatLoop
{
    public class PlayerHandController : MonoBehaviour
    {
        [SerializeField] private CardContainer handContainer;
        [SerializeField] private CardMovement cardMovementPrefab;
        [SerializeField] private List<CardData> cardData;
        
        public CardContainer container => handContainer;
        
        public IEnumerator DrawHand()
        {
            yield return new WaitForSeconds(0.1f);
            DrawCard();
            yield return new WaitForSeconds(0.1f);
            DrawCard();
            yield return new WaitForSeconds(0.1f);
            DrawCard();
            yield return new WaitForSeconds(0.1f);
            DrawCard();
            yield return new WaitForSeconds(0.1f);
            DrawCard();
            yield return new WaitForSeconds(0.1f);
            DrawCard();
            yield return new WaitForSeconds(0.1f);
            DrawCard();
            yield return new WaitForSeconds(0.1f);
            DrawCard();
            yield return new WaitForSeconds(0.1f);
        }

        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame && CombatLoop.instance.currentTurn == CombatLoop.TurnType.Preparation)
            {
                StopAllCoroutines();
                StartCoroutine(DrawNewHand());
            }
        }
        
        private IEnumerator DrawNewHand()
        {
            int currentCardCount = handContainer.Slots.Count;

            for (int i = 0; i < currentCardCount; i++)
            {
                handContainer.Slots[0].CurrentCard.cardController.KillCard();
                yield return new WaitForSeconds(0.05f);
            }

            for (int i = 0; i < 8; i++)
            {
                DrawCard();
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void DrawCard()
        {
            CardMovement newCard = Instantiate(cardMovementPrefab);
            handContainer.ReceiveCard(newCard);

            int randomIndex = Random.Range(0, cardData.Count);
            CardController controller = CardsVisualManager.instance.SpawnNewCardVisuals(newCard, cardData[randomIndex]);
            newCard.SetCardController(controller);
        }
    }
}
