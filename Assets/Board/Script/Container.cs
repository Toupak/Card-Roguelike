using System;
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

        [SerializeField] private Transform separationLine;
        [SerializeField] private Container otherContainer;

        [SerializeField] private Slot slotPrefab;

        private CardController currentSelectedCard;

        private List<Slot> slots;

        public ContainerType type;

        public enum ContainerType
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

            CheckRelativePositionToOtherBoard();

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

        private void CheckRelativePositionToOtherBoard()
        {
            if (type == ContainerType.Hand && currentSelectedCard.position.y > separationLine.position.y)
                SendToOtherBoard();
            else if (type == ContainerType.Board && currentSelectedCard.position.y < separationLine.position.y)
                SendToOtherBoard();
        }

        private void SendToOtherBoard()
        {
            otherContainer.ReceiveCardFromOtherBoard(currentSelectedCard);
            DeleteCurrentSlot();
            currentSelectedCard = null;
        }

        public void ReceiveCardFromOtherBoard(CardController card)
        {
            currentSelectedCard = card;
            CreateNewSlot();
        }

        private void DeleteCurrentSlot()
        {
            Destroy(slots[currentSelectedCard.SlotIndex].gameObject);
            slots.RemoveAt(currentSelectedCard.SlotIndex);
        }

        private void CreateNewSlot()
        {
            Slot newSlot = Instantiate(slotPrefab);
            newSlot.transform.SetParent(transform);
            newSlot.Setup(slots.Count, this);
            slots.Add(newSlot);

            currentSelectedCard.SetNewSlot(newSlot);
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
