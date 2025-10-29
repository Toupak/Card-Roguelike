using Data;
using Spells;
using UnityEngine;
using UnityEngine.UI;

namespace Cards.Scripts
{
    public class CardController : MonoBehaviour
    {
        [SerializeField] private FollowTarget followTarget;
        [SerializeField] private Image artwork;
        [SerializeField] private SpellButton leftButton;
        [SerializeField] private SpellButton rightButton;

        private RectTransform rectTransform;
        public Vector2 screenPosition => rectTransform.position;
        
        public CardMovement cardMovement { get;  private set; }
        public CardData cardData { get;  private set; }

        public CardHealth cardHealth { get; private set; }
        public DisplayCardEffects displayCardEffect { get; private set; }

        public void Setup(CardMovement movement, CardData data)
        {
            rectTransform = GetComponent<RectTransform>();
            cardHealth = GetComponent<CardHealth>();
            cardHealth.OnDeath.AddListener(KillCard);
            displayCardEffect = GetComponent<DisplayCardEffects>();
            cardMovement = movement;
            cardData = data;

            if (data != null)
                artwork.sprite = data.artwork;
            
            followTarget.SetTarget(movement);
            
            if (data != null && data.leftSpell != null)
                leftButton.Setup(data.leftSpell, !movement.IsEnemyCard);
            else
                leftButton.Setup(null, !movement.IsEnemyCard);
            
            if (data != null && data.rightSpell != null)
                rightButton.Setup(data.rightSpell, !movement.IsEnemyCard);
            else
                rightButton.Setup(null, !movement.IsEnemyCard);

            if (cardHealth != null)
                cardHealth.Setup(data);
        }

        private void KillCard()
        {
            cardMovement.CurrentSlot.board.DeleteCurrentSlot(cardMovement.SlotIndex);
            Destroy(gameObject);
        }
    }
}
