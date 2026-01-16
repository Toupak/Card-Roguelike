using Items;
using Run_Loop;
using UnityEngine;

namespace Cards.Scripts
{
    public class CardsVisualManager : MonoBehaviour
    {
        [SerializeField] private CardController cardGraphicsPrefab;
        [SerializeField] private CardController tokenGraphicsPrefab;
        [SerializeField] private ItemController itemGraphicsPrefab;
        
        public static CardsVisualManager instance;

        private void Awake()
        {
            instance = this;
        }

        public CardController SpawnNewCardVisuals(CardMovement movement, DeckCard deckCard)
        {
            CardController prefab = deckCard.cardData != null && deckCard.cardData.alternativeCardPrefab != null ? deckCard.cardData.alternativeCardPrefab : cardGraphicsPrefab;
            
            CardController newCard = Instantiate(prefab, movement.transform.position, Quaternion.identity, transform);
            newCard.Setup(movement, deckCard);

            return newCard;
        }
        
        public CardController SpawnNewTokenVisuals(CardMovement movement, CardData tokenData, CardController parentCardController)
        {
            CardController newCard = Instantiate(tokenData.alternativeCardPrefab != null ? tokenData.alternativeCardPrefab : tokenGraphicsPrefab, movement.transform.position, Quaternion.identity, transform);
            newCard.SetupToken(movement, tokenData, parentCardController);

            return newCard;
        }
        
        public ItemController SpawnNewItemVisuals(CardMovement movement)
        {
            ItemController newItem = Instantiate(itemGraphicsPrefab, movement.transform.position, Quaternion.identity, transform);
            newItem.SetupItem(movement);

            return newItem;
        }
    }
}
