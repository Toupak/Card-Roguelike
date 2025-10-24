using System;
using CardSlot;

using UnityEngine;

namespace Cards.Scripts
{
    public class CardController : MonoBehaviour
    {
        [SerializeField] private CardMovement cardMovement;
        [SerializeField] private Transform cardGraphics;
        
        private Slot slot;
        public Slot Slot => slot;
        public int SlotIndex => slot.Index;

        //EN CAS DE BUG MYSTIQUE
        public Vector2 position => cardMovement.transform.position;

        private void Start()
        {
            slot = transform.parent.GetComponent<Slot>();
        }
        
        public void SetNewSlot(Slot newSlot, bool resetPosition = false)
        {
            Vector3 currentMovementPosition = position;
            Vector3 currentGraphicsPosition = cardGraphics.transform.position;
            
            slot = newSlot;
            transform.SetParent(slot.transform);
            transform.position = slot.transform.position;

            if (resetPosition)
                cardMovement.ResetPosition();
            else
                cardMovement.transform.position = currentMovementPosition;

            cardGraphics.transform.position = currentGraphicsPosition;
        }

        public void OnBeginDragging()
        {
            if (slot.board != null)
                slot.board.OnStartDragging.Invoke(this);
        }

        public void OnEndDrag()
        {
            if (slot.board != null)
                slot.board.OnStopDragging.Invoke();
        }
    }
}
