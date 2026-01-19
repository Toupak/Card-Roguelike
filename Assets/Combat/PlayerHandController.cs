using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Card_Container.Script;
using Run_Loop;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Combat
{
    public class PlayerHandController : MonoBehaviour
    {
        [SerializeField] private CardContainer handContainer;
        [SerializeField] private CardContainer playerBoard;
        [SerializeField] private CardMovement cardMovementPrefab;
        [SerializeField] private List<CardData> cardData;
        
        public CardContainer container => handContainer;

        private int currentDebugDeckIndex = 0;
        
        public IEnumerator DrawHand()
        {
            if (RunLoop.instance != null && RunLoop.instance.isInRun)
            {
                if (RunLoop.instance.currentBattleIndex == 0)
                    yield return DrawFirstFightHand();
                else
                    yield return DrawLastFightHand();
            }
            else
            {
                yield return DrawDebugModeHand();
            }
        }

        private IEnumerator DrawLastFightHand()
        {
            List<FrameItem> frames = PlayerInventory.instance.frames;
            List<DeckCard> lastHandCards = PlayerDeck.instance.lastHandPlayed;
            for (int i = 0; i < lastHandCards.Count; i++)
            {
                CardContainer targetContainer = i < 4 ? playerBoard : handContainer;
                CardController card = SpawnCard(lastHandCards[i], targetContainer);
                
                List<FrameItem> equippedFrame = frames.Where((f) => f.target == card.deckCard).ToList();
                if (equippedFrame.Count > 0)
                    card.AddFrame(equippedFrame[0].data);
                
                yield return new WaitForSeconds(0.1f);
            }

            List<DeckCard> deck = PlayerDeck.instance.deck;
            for (int i = 0; i < deck.Count; i++)
            {
                if (lastHandCards.Contains(deck[i]))
                    continue;
                
                SpawnCard(deck[i], handContainer);
                yield return new WaitForSeconds(0.1f);
            }
        }

        private IEnumerator DrawFirstFightHand()
        {
            List<DeckCard> cards = PlayerDeck.instance.deck;

            for (int i = 0; i < cards.Count; i++)
            {
                CardContainer targetContainer = i < 4 ? playerBoard : handContainer;
                
                SpawnCard(cards[i], targetContainer);
                yield return new WaitForSeconds(0.1f);
            }
        }

        private IEnumerator DrawDebugModeHand()
        {
            for (int i = 0; i < 12; i++)
            {
                SpawnCard(new DeckCard(cardData[currentDebugDeckIndex]), handContainer);
                currentDebugDeckIndex = currentDebugDeckIndex + 1 >= cardData.Count ? 0 : currentDebugDeckIndex + 1;

                yield return new WaitForSeconds(0.1f);
            } 
        }

        public CardController SpawnCard(DeckCard deckCard, CardContainer targetContainer)
        {
            CardMovement newCard = Instantiate(cardMovementPrefab);
            targetContainer.ReceiveCard(newCard);

            CardController controller = CardsVisualManager.instance.SpawnNewCardVisuals(newCard, deckCard);
            newCard.SetCardController(controller);

            return controller;
        }
        
        public CardController SpawnToken(SpawnCardGA spawnCardGa)
        {
            CardMovement newCard = Instantiate(cardMovementPrefab);
            spawnCardGa.spawner.cardMovement.tokenContainer.ReceiveCard(newCard);

            CardController controller = CardsVisualManager.instance.SpawnNewTokenVisuals(newCard, spawnCardGa.cardData, spawnCardGa.spawner.cardMovement.cardController);
            newCard.SetCardController(controller);

            return controller;
        }
        
        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame && !RunLoop.instance.isInRun && CombatLoop.instance.currentTurn == CombatLoop.TurnType.Preparation)
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

            for (int i = 0; i < 12; i++)
            {
                SpawnCard(new DeckCard(cardData[currentDebugDeckIndex]), handContainer);
                currentDebugDeckIndex = currentDebugDeckIndex + 1 >= cardData.Count ? 0 : currentDebugDeckIndex + 1;
                yield return new WaitForSeconds(0.1f);
            }
        }

        public void DeactivateHand()
        {
            for (int i = playerBoard.Slots.Count - 1; i >= 0; i--)
            {
                if (playerBoard.Slots[i].CurrentCard.cardController == null)
                    playerBoard.Slots[i].CurrentCard.KillCard();
            }
            
            for (int i = handContainer.Slots.Count - 1; i >= 0; i--)
            {
                handContainer.Slots[i].CurrentCard.cardController.KillCard(false);
            }
            
            handContainer.gameObject.SetActive(false);
        }
    }
}
