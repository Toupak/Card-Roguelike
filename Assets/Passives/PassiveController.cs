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
    }
}
