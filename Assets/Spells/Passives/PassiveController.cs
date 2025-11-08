using Cards.Scripts;
using UnityEngine;

namespace Spells.Passives
{
    public class PassiveController : MonoBehaviour
    {
        protected CardController cardController;
        protected PassiveData passiveData;
        
        public virtual void Setup(CardController controller, PassiveData data)
        {
            cardController = controller;
            passiveData = data;
        }
    }
}
