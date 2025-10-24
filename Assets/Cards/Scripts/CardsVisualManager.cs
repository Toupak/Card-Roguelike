using UnityEngine;

namespace Cards.Scripts
{
    public class CardsVisualManager : MonoBehaviour
    {
        [SerializeField] private GameObject cardGraphicsPrefab;
        
        public static CardsVisualManager instance;

        private void Awake()
        {
            instance = this;
        }

        public void SpawnNewCardVisuals(CardMovement movement, CardData data)
        {
            GameObject newCard = Instantiate(cardGraphicsPrefab, movement.transform.position, Quaternion.identity, transform);
            newCard.GetComponent<FollowTarget>().SetTarget(movement);
        }
    }
}
