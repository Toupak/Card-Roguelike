using System.Collections.Generic;
using Cards.Scripts;
using CardSlot;
using UnityEngine;
using UnityEngine.Events;

namespace Board.Script
{
    public class Container : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<CardController> OnStartDragging = new UnityEvent<CardController>();
        [HideInInspector] public UnityEvent OnStopDragging = new UnityEvent();

        private CardController currentSelectedCard;

        private List<Slot> slots;

        public enum BoardType
        {
            Hand,
            Board
        }

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
            slots = new List<Slot>();
            for (int i = 0; i < transform.childCount; i++)
            {
                Slot slotContainer = transform.GetChild(i).GetComponent<Slot>();
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
