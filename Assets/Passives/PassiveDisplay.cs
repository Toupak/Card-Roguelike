using Cards.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Passives
{
    public class PassiveDisplay : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image background;

        public CardController cardController { get; private set; }
        public PassiveController passiveController { get; private set; }
        public PassiveData data { get; private set; }

        public PassiveController Setup(CardController CardController, PassiveData passiveData)
        {
            data = passiveData;
            background.color = passiveData.backgroundColor;
            cardController = CardController;
            
            if (passiveData.icon != null)
                icon.sprite = passiveData.icon;
            
            return SetupPassiveController(passiveData);
        }
        
        private PassiveController SetupPassiveController(PassiveData passiveData)
        {
            if (data.passiveController != null)
            {
                passiveController = Instantiate(data.passiveController, transform);
                passiveController.Setup(cardController, passiveData);
                return passiveController;
            }

            return null;
        }
    }
}
