using Cards.Scripts;
using Combat.Status.Data;
using UnityEngine;

namespace Combat.Status
{
    public class StatusController : MonoBehaviour
    {
        public CardController cardController { get; private set; }
        public StatusData statusData { get; private set; }
        
        protected StatusType statusType { get; private set; }
        
        public virtual void Setup(CardController controller, StatusData data)
        {
            cardController = controller;
            statusData = data;
            statusType = data.type;
        }
        
        public virtual void Remove()
        {
            Destroy(gameObject);
        }
        
        protected virtual int GetStackCount()
        {
            return cardController.cardStatus.GetCurrentStackCount(statusType);
        }
        
        public virtual void AddStack(int amount) { }
        public virtual void RemoveStack(int amount) { }
    }
}
