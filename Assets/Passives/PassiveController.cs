using System.Collections.Generic;
using Cards.Scripts;
using UnityEngine;

namespace Passives
{
    public class PassiveController : MonoBehaviour
    {
        protected CardController cardController;
        public PassiveData passiveData { get; private set; }
        
        public virtual void Setup(CardController controller, PassiveData data)
        {
            cardController = controller;
            passiveData = data;
        }
        
        protected CardController PickRandomTarget(List<CardMovement> targets)
        {
            return targets[Random.Range(0, targets.Count)].cardController;
        }
    }
}
