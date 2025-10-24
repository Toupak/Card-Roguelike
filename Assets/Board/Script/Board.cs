using System.Collections.Generic;
using Cards.Scripts;
using Slot;
using UnityEngine;
using UnityEngine.Events;

namespace Board.Script
{
    public class Board : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<CardController> OnStartDragging = new UnityEvent<CardController>();
        [HideInInspector] public UnityEvent OnStopDragging = new UnityEvent();

        private CardController currentSelectedCard;

        private List<SlotContainer> slots;

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
            slots = new List<SlotContainer>();
            for (int i = 0; i < transform.childCount; i++)
            {
                SlotContainer slotContainer = transform.GetChild(i).GetComponent<SlotContainer>();
                slotContainer.Setup(i, this);
                slots.Add(slotContainer);
            }
        }

        private void Update()
        {
            if (currentSelectedCard == null)
                return;

            for (int i = 0; i < slots.Count; i++)
            {
                if (currentSelectedCard.position.x > slots[i].transform.position.x)
                    if (currentSelectedCard.SlotIndex < i)
                    {
                        SwapSlots(i);
                        return;
                    }

                if (currentSelectedCard.position.x < slots[i].transform.position.x)
                    if (currentSelectedCard.SlotIndex > i)
                    {
                        SwapSlots(i);
                        return;
                    }
            }
        }

        private void SwapSlots(int slotToMoveIndex)
        {
            int temp = currentSelectedCard.SlotIndex;
            CardController cardToMove = GetCardFromSlotIndex(slotToMoveIndex);
            
            currentSelectedCard.SetNewSlot(slots[slotToMoveIndex]);
            cardToMove.SetNewSlot(slots[temp], true);
        }

        private CardController GetCardFromSlotIndex(int slotIndex)
        {
            return slots[slotIndex].transform.GetChild(0).GetComponent<CardController>();
        }

        private void StartDraggingNewCard(CardController newCard)
        {
            currentSelectedCard = newCard;
        }

        private void StopDraggingCard()
        {
            currentSelectedCard = null;
        }
    }
}
