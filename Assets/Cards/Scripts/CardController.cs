using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CardController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private TextMeshPro cardName;
    [SerializeField] private TextMeshPro cardNumber;

    private bool isDragging;
    public bool IsDragging => isDragging;

    private Board board;

    private void Start()
    {
        board = transform.parent.parent.GetComponent<Board>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;

        if (board != null)
            board.OnStartDragging.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero;

        isDragging = false;

        if (board != null)
            board.OnStopDragging.Invoke();
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

    public void Setup(CardData cardData)
    {
        cardName.text = cardData.cardName;
        cardNumber.text = cardData.cardNumber.ToString();
    }
}
