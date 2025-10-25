using UnityEngine;

namespace CardSlot
{
    public class Slot : MonoBehaviour
    {
        [HideInInspector] public Board.Script.Container board; 
        
        public bool IsEmpty => transform.childCount < 1;
        
        public void Setup(int slotIndex, Board.Script.Container parentBoard)
        {
            board = parentBoard;
        }
    }
}
