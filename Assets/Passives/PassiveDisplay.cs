using Cards.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Passives
{
    public class PassiveDisplay : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image background;
        [SerializeField] private PassiveController defaultPassiveController;

        public CardController cardController { get; private set; }
        public PassiveController passiveController { get; private set; }
        public PassiveData data { get; private set; }

        public PassiveController Setup(CardController CardController, PassiveData passiveData, int index)
        {
            data = passiveData;
            background.color = passiveData.backgroundColor;
            cardController = CardController;
            
            if (passiveData.icon != null)
                icon.sprite = passiveData.icon;
            
            return SetupPassiveController(passiveData, index);
        }
        
        private PassiveController SetupPassiveController(PassiveData passiveData, int index)
        {
            passiveController = Instantiate(data.passiveController != null ? data.passiveController : defaultPassiveController, transform);
            passiveController.Setup(cardController, passiveData, index);
            return passiveController;
        }
    }
}
