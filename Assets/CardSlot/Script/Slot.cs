using UnityEngine;

namespace CardSlot
{
    public class Slot : MonoBehaviour
    {
        [HideInInspector] public Board.Script.Container board; 
        
        private int index;
        public int Index => index;

        public bool IsEmpty => transform.childCount < 1;
        
        public void Setup(int slotIndex, Board.Script.Container parentBoard)
        {
            index = slotIndex;
            board = parentBoard;
        }

        public void SetIndex(int slotIndex)
        {
            index = slotIndex;
        }
    }
}
