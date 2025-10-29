using Data;
using Spells;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cards.Scripts
{
    public class CardController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI cardName;
        [SerializeField] private Image artwork;
        [SerializeField] public SpellButton leftButton;
        [SerializeField] public SpellButton rightButton;

        private RectTransform rectTransform;
        public Vector2 screenPosition => rectTransform.position;
        
        private FollowTarget followTarget;
        public CardMovement cardMovement { get;  private set; }
        public CardData cardData { get;  private set; }

        public CardHealth cardHealth { get; private set; }
        public DisplayCardEffects displayCardEffect { get; private set; }

        public void Setup(CardMovement movement, CardData data)
        {
            rectTransform = GetComponent<RectTransform>();
            followTarget = GetComponent<FollowTarget>();
            followTarget.SetTarget(movement);
            
            cardHealth = GetComponent<CardHealth>();
            cardHealth.Setup(data);
            cardHealth.OnDeath.AddListener(KillCard);
            
            displayCardEffect = GetComponent<DisplayCardEffects>();
            
            cardMovement = movement;
            cardData = data;

            cardName.text = data.cardName;
            artwork.sprite = data.artwork;
            gameObject.name = data.cardName;
            
            leftButton.Setup(this, data.leftSpell, !movement.IsEnemyCard);
            rightButton.Setup(this, data.rightSpell, !movement.IsEnemyCard);
        }

        private void KillCard()
        {
            cardMovement.CurrentSlot.board.DeleteCurrentSlot(cardMovement.SlotIndex);
            Destroy(gameObject);
        }
    }
}
