using Data;
using Run_Loop;
using UnityEngine;

namespace Cards.Scripts
{
    public class CardsVisualManager : MonoBehaviour
    {
        [SerializeField] private CardController cardGraphicsPrefab;
        [SerializeField] private CardController tokenGraphicsPrefab;
        
        public static CardsVisualManager instance;

        private void Awake()
        {
            instance = this;
        }

        public CardController SpawnNewCardVisuals(CardMovement movement, DeckCard deckCard)
        {
            CardController newCard = Instantiate(deckCard.cardData.alternativeCardPrefab != null ? deckCard.cardData.alternativeCardPrefab : cardGraphicsPrefab, movement.transform.position, Quaternion.identity, transform);
            newCard.Setup(movement, deckCard);

            return newCard;
        }
        
        public CardController SpawnNewTokenVisuals(CardMovement movement, CardData tokenData)
        {
            CardController newCard = Instantiate(tokenData.alternativeCardPrefab != null ? tokenData.alternativeCardPrefab : tokenGraphicsPrefab, movement.transform.position, Quaternion.identity, transform);
            newCard.Setup(movement, tokenData);

            return newCard;
        }
    }
}
