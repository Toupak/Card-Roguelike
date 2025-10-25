using System.Collections.Generic;
using Cards.Scripts;
using CardSlot;
using Cursor.Script;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Board.Script
{
    public class Container : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<CardMovement> OnStartDragging = new UnityEvent<CardMovement>();
        [HideInInspector] public UnityEvent OnStopDragging = new UnityEvent();

        [SerializeField] private int maxCardCount;
        
        [Space]
        [SerializeField] private Slot slotPrefab;
        [SerializeField] private CardMovement cardMovementPrefab;

        private CardMovement currentSelectedCard;

        private List<Slot> slots = new List<Slot>();

        public ContainerType type;

        public enum ContainerType
        {
            Hand,
            Board,
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
            if (type == ContainerType.Hand) //TODO move it to PlayerHandController
                CheckDrawCard();
            
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

        private void CheckDrawCard()//TODO move it to PlayerHandController
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame && !IsFull)
                DrawCard();
        }

        private void DrawCard()//TODO move it to PlayerHandController
        {
            CardMovement newCard = Instantiate(cardMovementPrefab);
            ReceiveCard(newCard);
            CardsVisualManager.instance.SpawnNewCardVisuals(newCard, null);
        }

        private bool CheckForSendingCardToOtherContainer()
        {
            Container currentCursorContainer = CursorInfo.instance.lastContainer;

            if (currentCursorContainer != this && !currentCursorContainer.IsFull)
            {
                SendToOtherBoard(currentCursorContainer);
                return true;
            }

            return false;
        }

        private void SendToOtherBoard(Container currentCursorContainer)
        {
            int currentIndex = currentSelectedCard.SlotIndex;
            currentCursorContainer.ReceiveCardFromOtherBoard(currentSelectedCard);
            DeleteCurrentSlot(currentIndex);
            currentSelectedCard = null;
        }
        
        private void DeleteCurrentSlot(int index)
        {
            Destroy(slots[index].gameObject);
            slots.RemoveAt(index);
        }

        public void ReceiveCard(CardMovement card)
        {
            card.SetNewSlot(CreateNewSlot(), true);
        }

        public void ReceiveCardFromOtherBoard(CardMovement card)
        {
            currentSelectedCard = card;
            currentSelectedCard.SetNewSlot(CreateNewSlot(), false);
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
