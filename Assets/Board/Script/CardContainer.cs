using System;
using System.Collections.Generic;
using BoomLib.Tools;
using Cards.Scripts;
using CardSlot.Script;
using Cursor.Script;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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

        private HorizontalLayoutGroup horizontalLayoutGroup;
        
        private CardMovement currentSelectedCard;

        private List<Slot> slots = new List<Slot>();
        public List<Slot> Slots => slots;
        public int slotCount => slots.Count;

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

        private void OnEnable()
        {
            OnStartDragging.AddListener(StartDraggingNewCard);
            OnStopDragging.AddListener(StopDraggingCard);
            OnAnyContainerUpdated.AddListener(UpdateCardSpacing);
        }

        private void OnDisable()
        {
            OnStartDragging.RemoveListener(StartDraggingNewCard);
            OnStopDragging.RemoveListener(StopDraggingCard);
            OnAnyContainerUpdated.RemoveListener(UpdateCardSpacing);
        }

        private void Start()
        {
            horizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
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
        
        public bool IsFull()
        {
            if (type == ContainerType.Board)
            {
                if (CombatLoop.CombatLoop.instance.currentTurn == CombatLoop.CombatLoop.TurnType.Preparation)
                    return slots.Count >= maxCardCount;
                else
                    return slots.Count >= 12;
            }
            
            return slots.Count >= maxCardCount;
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
            
            if (currentCursorCardContainer != this && !currentCursorCardContainer.IsFull() && currentCursorCardContainer.type != ContainerType.Enemy)
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
        
        private Slot CreateNewSlot()
        {
            Slot newSlot = Instantiate(slotPrefab, transform);
            newSlot.Setup(slots.Count, this);
            slots.Add(newSlot);

            return newSlot;
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
        
        private void UpdateCardSpacing()
        {
            if (type != ContainerType.Board || horizontalLayoutGroup == null)
                return;

            if (slotCount <= 4)
                horizontalLayoutGroup.spacing = 250.0f;
            else
                horizontalLayoutGroup.spacing = Mathf.Clamp(Tools.NormalizeValueInRange(slotCount, 4.0f, 12.0f, 250.0f, 110.0f), 110.0f, 250.0f);
        }

        public void ResetContainer()
        {
            for (int i = slots.Count - 1; i >= 0; i--)
            {
                if (!slots[i].IsEmpty)
                    slots[i].CurrentCard.cardController.KillCard(false);
            }
        }
    }
}
