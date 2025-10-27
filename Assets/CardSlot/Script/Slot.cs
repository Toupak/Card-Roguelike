using UnityEngine;

namespace CardSlot
{
    public class Slot : MonoBehaviour
    {
        [HideInInspector] public Board.Script.CardContainer board; 
        
        public bool IsEmpty => transform.childCount < 1;
        
        public void Setup(int slotIndex, Board.Script.CardContainer parentBoard)
        {
            board = parentBoard;
        }
    }
}
