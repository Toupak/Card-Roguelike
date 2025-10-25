using CardSlot;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cards.Scripts
{
    public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
    {
        private bool isDragging;
        public bool IsDragging => isDragging;

        private Slot slot;
        public int SlotIndex => slot.Index;

        public void SetNewSlot(Slot newSlot, bool resetPosition)
        {
            slot = newSlot;
            transform.SetParent(slot.transform);
            
            if (resetPosition)
                ResetPosition();
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            isDragging = true;
            slot.board.OnStartDragging?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ResetPosition();

            isDragging = false;
            slot.board.OnStopDragging?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {

        }
        public void OnPointerUp(PointerEventData eventData)
        {

        }

        public void OnPointerEnter(PointerEventData eventData)
        {

        }

        public void OnPointerExit(PointerEventData eventData)
        {

        }

        public void ResetPosition()
        {
            transform.localPosition = Vector3.zero;
        }
    }
}
