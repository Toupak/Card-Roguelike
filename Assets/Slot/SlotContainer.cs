using UnityEngine;

namespace Slot
{
    public class SlotContainer : MonoBehaviour
    {
        [HideInInspector] public Board.Script.Board board; 
        
        private int index;
        public int Index => index;

        public bool IsEmpty => transform.childCount < 1;
        
        public void Setup(int slotIndex, Board.Script.Board parentBoard)
        {
            index = slotIndex;
            board = parentBoard;
        }
    }
}
