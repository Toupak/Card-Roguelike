using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Board.Script;
using Cards.Scripts;
using Run_Loop;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatLoop
{
    public class PlayerHandController : MonoBehaviour
    {
        [SerializeField] private CardContainer handContainer;
        [SerializeField] private CardContainer playerBoard;
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
                    SpawnCard(deckCard, handContainer);
                    yield return new WaitForSeconds(0.1f);
                }    
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    SpawnCard(new DeckCard(cardData[Random.Range(0, cardData.Count)]), handContainer);
                    yield return new WaitForSeconds(0.1f);
                }   
            }
        }

        public void SpawnCard(DeckCard deckCard, CardContainer targetContainer)
        {
            CardMovement newCard = Instantiate(cardMovementPrefab);
            targetContainer.ReceiveCard(newCard);

            CardController controller = CardsVisualManager.instance.SpawnNewCardVisuals(newCard, deckCard);
            newCard.SetCardController(controller);
        }
        
        public void SpawnToken(SpawnCardGA spawnCardGa)
        {
            CardMovement newCard = Instantiate(cardMovementPrefab);
            spawnCardGa.spawner.cardMovement.tokenContainer.ReceiveCard(newCard);

            CardController controller = CardsVisualManager.instance.SpawnNewTokenVisuals(newCard, spawnCardGa.cardData, spawnCardGa.spawner.cardMovement.cardController);
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
                SpawnCard(new DeckCard(cardData[Random.Range(0, cardData.Count)]), handContainer);
                yield return new WaitForSeconds(0.1f);
            }
        }

        public void DeactivateHand()
        {
            for (int i = handContainer.Slots.Count - 1; i >= 0; i--)
            {
                handContainer.Slots[i].CurrentCard.cardController.KillCard();
            }
            
            handContainer.gameObject.SetActive(false);
        }
    }
}
