using Data;
using Spells;
using UnityEngine;

namespace Cards.Scripts
{
    public class CardController : MonoBehaviour
    {
        [SerializeField] private FollowTarget followTarget;
        [SerializeField] private SpellButton leftButton;
        [SerializeField] private SpellButton rightButton;
        
        public CardMovement cardMovement { get;  private set; }
        public CardData cardData { get;  private set; }

        public void Setup(CardMovement movement, CardData data)
        {
            cardMovement = movement;
            cardData = data;
            
            followTarget.SetTarget(movement);
            
            if (data != null && data.leftSpell != null)
                leftButton.Setup(data.leftSpell, !movement.IsEnemyCard);
            else
                leftButton.Setup(null, !movement.IsEnemyCard);
            
            if (data != null && data.rightSpell != null)
                rightButton.Setup(data.rightSpell, !movement.IsEnemyCard);
            else
                rightButton.Setup(null, !movement.IsEnemyCard);
        }
    }
}
