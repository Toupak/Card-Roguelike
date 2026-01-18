using System;
using Board.Script;
using BoomLib.Tools;
using CardSlot.Script;
using Cursor.Script;
using Items;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Cards.Scripts
{
    public class CardMovement : MonoBehaviour
    {
        [SerializeField] public CardContainer tokenContainer;
        
        [HideInInspector] public UnityEvent OnHover = new UnityEvent();
        [HideInInspector] public UnityEvent OnSelected = new UnityEvent();
        [HideInInspector] public UnityEvent OnDeselected = new UnityEvent();
        [HideInInspector] public UnityEvent OnStartDrag = new UnityEvent();
        [HideInInspector] public UnityEvent OnDrop = new UnityEvent();
        [HideInInspector] public UnityEvent OnSetNewSlot = new UnityEvent();

        public bool isDragging { get;  private set; }
        public bool isHovering { get;  private set; }
        public bool isInspected { get;  private set; }

        public RectTransform rectTransform { get;  private set; }
        public CardController cardController { get;  private set; }
        public ItemController itemController { get;  private set; }
        private Slot slot;
        public Slot CurrentSlot => slot;
        public int SlotIndex => transform.parent.CompareTag("Slot") ? transform.parent.GetSiblingIndex() : 0;
        public int SlotSiblingCount => transform.parent.CompareTag("Slot") ? transform.parent.parent.childCount - 1 : 0;
        public Vector3 SlotPosition => slot.transform.position;
        public CardContainer.ContainerType ContainerType => slot.board.type;
        public bool IsEnemyCard => ContainerType == CardContainer.ContainerType.Enemy;

        private bool isCursorFree => CursorInfo.instance.currentMode == CursorInfo.CursorMode.Free;
        public bool canBeDragged => !IsEnemyCard && isCursorFree;
        public bool canBeInspected => true;

        private Vector2 startingSize;
        private Vector2 scaleVelocity;
        
        private void Start()
        {
            if (rectTransform == null)
                rectTransform = GetComponent<RectTransform>();

            startingSize = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
        }

        private void LateUpdate()
        {
            if (isHovering && CursorInfo.instance.currentCardMovement != this)
                isHovering = false;

            if (isDragging)
                transform.position = Tools.ClampPositionInScreen(CursorInfo.instance.currentPosition, rectTransform.rect.size);
            
            if (isInspected)
                transform.position = Tools.ClampPositionInScreen(SlotPosition, rectTransform.rect.size);

            UpdateScale();
        }

        private void UpdateScale()
        {
            Vector2 targetScale = startingSize * (isInspected ? 2.0f : 1.0f);

            Vector2 currentScale = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
            Vector2 newScale = Vector2.SmoothDamp(currentScale, targetScale, ref scaleVelocity, 0.15f);

            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newScale.x);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newScale.y);
        }

        public void SetNewSlot(Slot newSlot, bool resetPosition)
        {
            if (rectTransform == null)
                rectTransform = GetComponent<RectTransform>();
            
            slot = newSlot;
            transform.SetParent(slot.transform, true);
            transform.localScale = Vector3.one;
            
            if (resetPosition)
                ResetPosition();
            
            OnSetNewSlot?.Invoke();
        }
        
        public void OnBeginDrag()
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

        public void OnEndDrag()
        {
            isDragging = false;

            slot.board.OnStopDragging?.Invoke();
            
            Deselect();
            ResetPosition();
            
            OnDrop?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (CursorInfo.instance.currentCardMovement != this)
                return;
            
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

        public void OnInspect()
        {
            isInspected = true;
            OnSelected?.Invoke();
        }

        public void OnDeselect()
        {
            Deselect(false);
            ResetPosition();
        }

        private void Deselect(bool forceDeselect = true)
        {
            if (!isInspected)
                return;
            
            isInspected = false;
            if (forceDeselect)
                EventSystem.current.SetSelectedGameObject(null);
            OnDeselected?.Invoke();
        }

        public void SetCardController(CardController newController)
        {
            cardController = newController;
        }

        public void SetItemController(ItemController newController)
        {
            itemController = newController;
        }

        public void KillAllTokens()
        {
            for (int i = tokenContainer.Slots.Count - 1; i >= 0; i--)
            {
                tokenContainer.Slots[i].CurrentCard.cardController.KillCard();
            }
        }

        private void OnEnable()
        {
            if (cardController != null)
                cardController.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            if (cardController != null)
                cardController.gameObject.SetActive(false);
        }

        public void KillCard(bool removeFromDeck = true)
        {
            if (cardController != null)
                cardController.KillCard(removeFromDeck);
            else if (itemController != null)
                itemController.KillItem(removeFromDeck);
        }
    }
}
