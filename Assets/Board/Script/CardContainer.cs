using System.Collections.Generic;
using Cards.Scripts;
using CardSlot;
using Cursor.Script;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Board.Script
{
    public class CardContainer : MonoBehaviour
    {
        public static UnityEvent OnAnyContainerUpdated = new UnityEvent();
        
        [HideInInspector] public UnityEvent<CardMovement> OnStartDragging = new UnityEvent<CardMovement>();
        [HideInInspector] public UnityEvent OnStopDragging = new UnityEvent();

        [SerializeField] private int maxCardCount;
        
        [Space]
        [SerializeField] private Slot slotPrefab;
        [SerializeField] private CardMovement cardMovementPrefab;

        private CardMovement currentSelectedCard;

        private List<Slot> slots = new List<Slot>();
        public List<Slot> Slots => slots;

        public ContainerType type;

        public enum ContainerType
        {
            Hand,
            Board,
            Enemy,
            Sticky
        }

        public bool IsFull => slots.Count >= maxCardCount; 

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

        private void Update()
        {
            if (currentSelectedCard == null)
                return;

            if (CheckForSendingCardToOtherContainer())
                return;
                
            for (int i = 0; i < slots.Count; i++)
            {
                if (currentSelectedCard.transform.position.x > slots[i].transform.position.x)
                    if (currentSelectedCard.SlotIndex < i)
                    {
                        SwapSlots(i);
                        return;
                    }

                if (currentSelectedCard.transform.position.x < slots[i].transform.position.x)
                    if (currentSelectedCard.SlotIndex > i)
                    {
                        SwapSlots(i);
                        return;
                    }
            }
        }

        private bool CheckForSendingCardToOtherContainer()
        {
            CardContainer currentCursorCardContainer = CursorInfo.instance.LastCardContainer;

            if (currentCursorCardContainer != this && !currentCursorCardContainer.IsFull && currentCursorCardContainer.type != ContainerType.Enemy)
            {
                SendToOtherBoard(currentCursorCardContainer);
                return true;
            }

            return false;
        }

        private void SendToOtherBoard(CardContainer currentCursorCardContainer)
        {
            int currentIndex = currentSelectedCard.SlotIndex;
            currentCursorCardContainer.ReceiveCardFromOtherBoard(currentSelectedCard);
            DeleteCurrentSlot(currentIndex);
            currentSelectedCard = null;
        }
        
        private void DeleteCurrentSlot(int index)
        {
            Destroy(slots[index].gameObject);
            slots.RemoveAt(index);
            
            OnAnyContainerUpdated?.Invoke();
        }

        public void ReceiveCard(CardMovement card)
        {
            card.SetNewSlot(CreateNewSlot(), true);
            
            OnAnyContainerUpdated?.Invoke();
        }

        public void ReceiveCardFromOtherBoard(CardMovement card)
        {
            currentSelectedCard = card;
            currentSelectedCard.SetNewSlot(CreateNewSlot(), false);
            
            OnAnyContainerUpdated?.Invoke();
        }
        
        private Slot CreateNewSlot()
        {
            Slot newSlot = Instantiate(slotPrefab, transform);
            newSlot.Setup(slots.Count, this);
            slots.Add(newSlot);

            return newSlot;
        }

        private void SwapSlots(int slotToMoveIndex)
        {
            int temp = currentSelectedCard.SlotIndex;
            CardMovement cardToMove = GetCardFromSlotIndex(slotToMoveIndex);
            
            currentSelectedCard.SetNewSlot(slots[slotToMoveIndex], false);
            cardToMove.SetNewSlot(slots[temp], true);
        }

        private CardMovement GetCardFromSlotIndex(int slotIndex)
        {
            return slots[slotIndex].transform.GetChild(0).GetComponent<CardMovement>();
        }

        private void StartDraggingNewCard(CardMovement newCard)
        {
            currentSelectedCard = newCard;
        }

        private void StopDraggingCard()
        {
            currentSelectedCard = null;
        }
    }
}
