using Cards.Scripts;
using Combat.Card_Container.Script;
using UnityEngine;

namespace Combat.Card_Container.CardSlot
{
    public class Slot : MonoBehaviour
    {
        [HideInInspector] public CardContainer board; 
        
        public bool IsEmpty => transform.childCount < 1;

        public CardMovement CurrentCard => transform.GetComponentInChildren<CardMovement>();
        
        public void Setup(int slotIndex, CardContainer parentBoard)
        {
            board = parentBoard;
        }
    }
}
