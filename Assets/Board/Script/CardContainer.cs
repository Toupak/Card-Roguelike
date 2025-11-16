using System.Collections.Generic;
using System.Linq;
using Cards.Scripts;
using CardSlot.Script;
using Cursor.Script;
using UnityEngine;
using UnityEngine.Events;

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

        public bool isLocked;

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
        
        public enum PreferredPosition
        {
            None,
            Left,
            Right,
            Center
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
                        SwapSlots(currentSelectedCard, i);
                        return;
                    }

                if (currentSelectedCard.transform.position.x < slots[i].transform.position.x)
                    if (currentSelectedCard.SlotIndex > i)
                    {
                        SwapSlots(currentSelectedCard, i);
                        return;
                    }
            }
        }

        public void SendCardToOtherBoard(int slot, CardContainer otherContainer)
        {
            otherContainer.ReceiveCardFromOtherBoard(slots[slot].CurrentCard, true);
            DeleteCurrentSlot(slot);
            currentSelectedCard = null;
        }

        private bool CheckForSendingCardToOtherContainer()
        {
            if (isLocked)
                return false;
            
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
            currentCursorCardContainer.ReceiveCardFromOtherBoard(currentSelectedCard, false);
            DeleteCurrentSlot(currentIndex);
            currentSelectedCard = null;
        }
        
        public void DeleteCurrentSlot(int index)
        {
            Destroy(slots[index].gameObject);
            slots.RemoveAt(index);
            
            OnAnyContainerUpdated?.Invoke();
        }

        public void ReceiveCard(CardMovement card, PreferredPosition preferredPosition = PreferredPosition.None)
        {
            card.SetNewSlot(CreateNewSlot(), true);

            if (preferredPosition != PreferredPosition.None)
                MoveCardToPreferredPosition(card, preferredPosition);
            
            OnAnyContainerUpdated?.Invoke();
        }

        public void ReceiveCardFromOtherBoard(CardMovement card, bool resetPosition)
        {
            card.SetNewSlot(CreateNewSlot(), resetPosition);

            currentSelectedCard = resetPosition ? null : card;
            
            OnAnyContainerUpdated?.Invoke();
        }
        
        private Slot CreateNewSlot()
        {
            Slot newSlot = Instantiate(slotPrefab, transform);
            newSlot.Setup(slots.Count, this);
            slots.Add(newSlot);

            return newSlot;
        }
        
        private void MoveCardToPreferredPosition(CardMovement card, PreferredPosition preferredPosition)
        {
            if (slots.Count < 3)
                return;

            if (preferredPosition == PreferredPosition.Left)
            {
                //find left-most card that does not prefer to be left, swap with it
                for (int i = 0; i < slots.Count; i++)
                {
                    if (slots[i].CurrentCard.cardController != null && slots[i].CurrentCard.cardController.cardData.preferredPosition != PreferredPosition.Left)
                    {
                        SwapSlots(card, i, true);
                        break;
                    }
                }
            }

            if (preferredPosition != PreferredPosition.Center)
            {
                //find card that prefers to be center, swap it to the center
                for (int i = 0; i < slots.Count; i++)
                {
                    if (i != slots.Count / 2 && slots[i].CurrentCard.cardController != null && slots[i].CurrentCard.cardController.cardData.preferredPosition == PreferredPosition.Center)
                    {
                        SwapSlots(slots[i].CurrentCard, slots.Count / 2, true);
                        break;
                    }
                }
            }
            
            if (preferredPosition == PreferredPosition.Center)
            {
                //find center-most card that does not prefer to be center, swap with it
                int startingIndex = Mathf.Clamp((slots.Count / 2) - 1, 0, slots.Count);
                for (int i = startingIndex; i < slots.Count; i++)
                {
                    if (slots[i].CurrentCard.cardController != null && slots[i].CurrentCard.cardController.cardData.preferredPosition != PreferredPosition.Center)
                    {
                        SwapSlots(card, i, true);
                        break;
                    }
                }
            }
        }

        private void SwapSlots(CardMovement selectedCard, int slotToMoveIndex, bool resetPositionOfSelectedCard = false)
        {
            int temp = selectedCard.SlotIndex;
            CardMovement cardToMove = GetCardFromSlotIndex(slotToMoveIndex);
            
            selectedCard.SetNewSlot(slots[slotToMoveIndex], resetPositionOfSelectedCard);
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
