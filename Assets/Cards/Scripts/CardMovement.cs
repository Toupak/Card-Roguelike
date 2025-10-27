using Board.Script;
using CardSlot;
using Cursor.Script;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Cards.Scripts
{
    public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler, ISelectHandler, IDeselectHandler
    {
        [HideInInspector] public UnityEvent OnHover = new UnityEvent();
        [HideInInspector] public UnityEvent OnSelected = new UnityEvent();
        [HideInInspector] public UnityEvent OnDeselected = new UnityEvent();
        [HideInInspector] public UnityEvent OnStartDrag = new UnityEvent();
        [HideInInspector] public UnityEvent OnDrop = new UnityEvent();
        [HideInInspector] public UnityEvent OnSetNewSlot = new UnityEvent();
        
        private bool isDragging;
        public bool IsDragging => isDragging;
        
        private bool isHovering;
        public bool IsHovering => isHovering;

        private bool isSelected;
        public bool IsSelected => isSelected;

        
        private Slot slot;
        public int SlotIndex => transform.parent.CompareTag("Slot") ? transform.parent.GetSiblingIndex() : 0;
        public int SlotSiblingCount => transform.parent.CompareTag("Slot") ? transform.parent.parent.childCount - 1 : 0;
        public Vector3 SlotPosition => slot.transform.position;
        public CardContainer.ContainerType ContainerType => slot.board.type;
        public bool IsEnemyCard => ContainerType == CardContainer.ContainerType.Enemy;

        private bool isCursorFree => CursorInfo.instance.currentMode == CursorInfo.CursorMode.Free;
        private bool canBeDragged => !IsEnemyCard && isCursorFree;

        private Selectable selectable;

        private void Start()
        {
            selectable = GetComponent<Selectable>();
        }

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
            if (!canBeDragged)
            {
                Deselect();
                return;
            }
            
            isDragging = true;
            slot.board.OnStartDragging?.Invoke(this);
            Deselect();
            OnStartDrag?.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!canBeDragged)
            {
                Deselect();
                return;
            }
            
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ResetPosition();

            isDragging = false;
            Deselect();
            slot.board.OnStopDragging?.Invoke();
            OnDrop?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {

        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (EventSystem.current.currentSelectedGameObject == gameObject && isCursorFree)
            {
                isSelected = true;
                OnSelected?.Invoke();
            }
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

        public void OnSelect(BaseEventData eventData)
        {
            if (IsEnemyCard)
            {
                Deselect();
                return;
            }
        }

        public void OnDeselect(BaseEventData eventData)
        {
            Deselect(false);
        }

        private void Deselect(bool forceDeselect = true)
        {
            if (!isSelected)
                return;
            
            isSelected = false;
            if (forceDeselect)
                EventSystem.current.SetSelectedGameObject(null);
            OnDeselected?.Invoke();
        }
    }
}
