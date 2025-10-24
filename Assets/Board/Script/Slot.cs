using Cards.Scripts;
using UnityEngine;

namespace Board.Script
{
    public class Slot : MonoBehaviour
    {
        private CardMovement cardMovement;

        public bool IsEmpty => cardMovement == null;

        void Start()
        {
        
        }

        void Update()
        {
        
        }

        public void ReceiveCard(CardMovement card)
        {
            cardMovement = card;
        }

        public CardMovement RemoveCard()
        {
            CardMovement cardTemp = cardMovement;
            cardMovement = null;

            return cardTemp;
        }
    }
}
