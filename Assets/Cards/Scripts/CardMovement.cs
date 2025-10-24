using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cards.Scripts
{
    public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
    {
        private bool isDragging;
        public bool IsDragging => isDragging;

        private CardController cardController;

        private void Start()
        {
            cardController = transform.parent.GetComponent<CardController>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            isDragging = true;
            cardController.OnBeginDragging();
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ResetPosition();

            isDragging = false;
            cardController.OnEndDrag();
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
