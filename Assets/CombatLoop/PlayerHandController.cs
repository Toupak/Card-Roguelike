using System.Collections;
using System.Collections.Generic;
using Board.Script;
using Cards.Scripts;
using Data;
using Run_Loop;
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
            if (RunLoop.instance != null)
            {
                List<DeckCard> cards = PlayerDeck.instance.deck;

                foreach (DeckCard deckCard in cards)
                {
                    DrawCard(deckCard);
                    yield return new WaitForSeconds(0.1f);
                }    
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    DrawCard(new DeckCard(cardData[Random.Range(0, cardData.Count)]));
                    yield return new WaitForSeconds(0.1f);
                }   
            }
        }

        private void DrawCard(DeckCard deckCard)
        {
            CardMovement newCard = Instantiate(cardMovementPrefab);
            handContainer.ReceiveCard(newCard);

            CardController controller = CardsVisualManager.instance.SpawnNewCardVisuals(newCard, deckCard);
            newCard.SetCardController(controller);
        }
        
        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame && RunLoop.instance == null && CombatLoop.instance.currentTurn == CombatLoop.TurnType.Preparation)
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
                DrawCard(new DeckCard(cardData[Random.Range(0, cardData.Count)]));
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
