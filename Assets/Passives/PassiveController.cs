using System.Collections.Generic;
using Cards.Scripts;
using UnityEngine;

namespace Passives
{
    public class PassiveController : MonoBehaviour
    {
        public CardController cardController { get; private set; }
        public PassiveData passiveData { get; private set; }
        
        public virtual void Setup(CardController controller, PassiveData data)
        {
            cardController = controller;
            passiveData = data;
        }

        public virtual void Remove()
        {
            
        }
        
        protected CardController PickRandomTarget(List<CardMovement> targets)
        {
            if (targets.Count < 1)
                return null;
            
            return targets[Random.Range(0, targets.Count)].cardController;
        }
    }
}
