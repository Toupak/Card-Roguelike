using Data;
using UnityEngine;

namespace Cards.Scripts
{
    public class CardsVisualManager : MonoBehaviour
    {
        [SerializeField] private CardController cardGraphicsPrefab;
        
        public static CardsVisualManager instance;

        private void Awake()
        {
            instance = this;
        }

        public CardController SpawnNewCardVisuals(CardMovement movement, CardData data)
        {
            CardController newCard = Instantiate(cardGraphicsPrefab, movement.transform.position, Quaternion.identity, transform);
            newCard.Setup(movement, data);

            return newCard;
        }
    }
}
