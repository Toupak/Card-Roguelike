using Inventory.Items;
using UnityEngine;

namespace Cards.Scripts
{
    public class CardsVisualManager : MonoBehaviour
    {
        [SerializeField] private CardController cardGraphicsPrefab;
        [SerializeField] private CardController tokenGraphicsPrefab;
        [SerializeField] private ItemController frameGraphicsPrefab;
        [SerializeField] private ItemController consumableGraphicsPrefab;
        
        public static CardsVisualManager instance;

        private void Awake()
        {
            instance = this;
        }

        public CardController SpawnNewCardVisuals(CardMovement movement, CardData deckCard)
        {
            CardController prefab = deckCard != null && deckCard.alternativeCardPrefab != null ? deckCard.alternativeCardPrefab : cardGraphicsPrefab;
            
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
        
        public ItemController SpawnNewFrameVisuals(CardMovement movement)
        {
            ItemController newItem = Instantiate(frameGraphicsPrefab, movement.transform.position, Quaternion.identity, transform);
            newItem.SetupItem(movement);

            return newItem;
        }
        
        public ItemController SpawnNewConsumableVisuals(CardMovement movement)
        {
            ItemController newItem = Instantiate(consumableGraphicsPrefab, movement.transform.position, Quaternion.identity, transform);
            newItem.SetupItem(movement);

            return newItem;
        }
    }
}
