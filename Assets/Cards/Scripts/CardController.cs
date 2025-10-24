using System;
using Slot;
using UnityEngine;

namespace Cards.Scripts
{
    public class CardController : MonoBehaviour
    {
        [SerializeField] private CardMovement cardMovement;
        [SerializeField] private Transform cardGraphics;
        
        private SlotContainer slot;
        public SlotContainer Slot => slot;
        public int SlotIndex => slot.Index;

        public Vector2 position => cardMovement.transform.position;

        private void Start()
        {
            slot = transform.parent.GetComponent<SlotContainer>();
        }
        
        public void SetNewSlot(SlotContainer newSlot, bool resetPosition = false)
        {
            Vector3 currentPosition = position;
            
            slot = newSlot;
            transform.SetParent(slot.transform);
            transform.position = slot.transform.position;

            if (resetPosition)
                cardMovement.ResetPosition();
            else
                cardMovement.transform.position = currentPosition;

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
