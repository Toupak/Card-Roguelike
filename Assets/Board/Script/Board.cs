using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Board : MonoBehaviour
{
    [HideInInspector] public UnityEvent<CardController> OnStartDragging = new UnityEvent<CardController>();
    [HideInInspector] public UnityEvent OnStopDragging = new UnityEvent();

    private CardController currentSelectedCard;
    private int currentSelectedSlot;

    private List<Transform> slots;

    private void OnEnable()
    {
        OnStartDragging.AddListener(StartDraggingNewCard);
        OnStopDragging.AddListener(StopDraggingCard);
    }

    private void OnDisable()
    {
        OnStartDragging.RemoveAllListeners();
        OnStopDragging.RemoveAllListeners();
    }

    private void Start()
    {
        SetupSlotList();
    }

    private void SetupSlotList()
    {
        slots = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
            slots.Add(transform.GetChild(i));
    }

    void Update()
    {
        if (currentSelectedCard == null)
            return;

        for (int i = 0; i < slots.Count; i++)
        {
            if (currentSelectedCard.transform.position.x > slots[i].transform.position.x)
                if (currentSelectedSlot < i)
                {
                    SwapSlots(i);
                    return;
                }

            if (currentSelectedCard.transform.position.x < slots[i].transform.position.x)
                if (currentSelectedSlot > i)
                {
                    SwapSlots(i);
                    return;
                }
        }
    }

    private void SwapSlots(int newSlotIndex)
    {
        int temp = currentSelectedSlot;

        SetNewSlot(GetCardFromSlotIndex(currentSelectedSlot), newSlotIndex);
        SetNewSlot(GetCardFromSlotIndex(newSlotIndex), temp);
        SetNewPosition(GetCardFromSlotIndex(currentSelectedSlot));

        currentSelectedSlot = newSlotIndex;
    }

    private int GetSlotFromCard(CardController card)
    {
        return slots.IndexOf(card.transform.parent);
    }

    private Transform GetCardFromSlotIndex(int slotIndex)
    {
        return slots[slotIndex].transform.GetChild(0);
    }

    private void SetNewPosition(Transform card)
    {
        card.transform.localPosition = Vector3.zero;
    }

    private void SetNewSlot(Transform newCard, int newSlot)
    {
        newCard.SetParent(slots[newSlot].transform);
    }

    private void StartDraggingNewCard(CardController newCard)
    {
        currentSelectedCard = newCard;
        currentSelectedSlot = slots.IndexOf(currentSelectedCard.transform.parent);
    }

    private void StopDraggingCard()
    {
        currentSelectedCard = null;
    }

    //Check position carte par rapport à tous les slots
}
