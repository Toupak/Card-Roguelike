using Cards.Scripts;
using UnityEngine;

namespace CardSlot.Script
{
    public class Slot : MonoBehaviour
    {
        [HideInInspector] public Board.Script.CardContainer board; 
        
        public bool IsEmpty => transform.childCount < 1;

        public CardMovement CurrentCard => transform.GetComponentInChildren<CardMovement>();
        
        public void Setup(int slotIndex, Board.Script.CardContainer parentBoard)
        {
            board = parentBoard;
        }
    }
}
