using Board.Script;
using CardSlot;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Cards.Scripts
{
    public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
    {
        [HideInInspector] public UnityEvent OnHover = new UnityEvent();
        [HideInInspector] public UnityEvent OnStartDrag = new UnityEvent();
        [HideInInspector] public UnityEvent OnDrop = new UnityEvent();
        [HideInInspector] public UnityEvent OnSetNewSlot = new UnityEvent();
        
        private bool isDragging;
        public bool IsDragging => isDragging;
        
        private bool isHovering;
        public bool IsHovering => isHovering;

        private Slot slot;
        public int SlotIndex => transform.parent.CompareTag("Slot") ? transform.parent.GetSiblingIndex() : 0;
        public int SlotSiblingCount => transform.parent.CompareTag("Slot") ? transform.parent.parent.childCount - 1 : 0;
        public Vector3 SlotPosition => slot.transform.position;
        public Container.ContainerType ContainerType => slot.board.type;

        public void SetNewSlot(Slot newSlot, bool resetPosition)
        {
            slot = newSlot;
            transform.SetParent(slot.transform);
            
            if (resetPosition)
                ResetPosition();
            
            OnSetNewSlot?.Invoke();
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            isDragging = true;
            slot.board.OnStartDragging?.Invoke(this);
            OnStartDrag?.Invoke();
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
            OnDrop?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {

        }
        public void OnPointerUp(PointerEventData eventData)
        {

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isHovering = true;
            OnHover?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHovering = false;
        }

        public void ResetPosition()
        {
            transform.localPosition = Vector3.zero;
        }
    }
}
