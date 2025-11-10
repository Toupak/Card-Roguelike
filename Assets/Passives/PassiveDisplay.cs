using Cards.Scripts;
using Tooltip;
using UnityEngine;
using UnityEngine.UI;

namespace Passives
{
    public class PassiveDisplay : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image background;

        private PassiveController passiveController;
        private PassiveData data;

        public PassiveController Setup(CardController cardController, PassiveData passiveData)
        {
            data = passiveData;
            background.color = passiveData.backgroundColor;
            
            if (passiveData.icon != null)
                icon.sprite = passiveData.icon;
            
            SetupTooltip(passiveData);
            return SetupPassiveController(cardController, passiveData);
        }
        
        private PassiveController SetupPassiveController(CardController cardController, PassiveData passiveData)
        {
            if (data.passiveController != null)
            {
                passiveController = Instantiate(data.passiveController, transform);
                passiveController.Setup(cardController, passiveData);
                return passiveController;
            }

            return null;
        }
        
        private void SetupTooltip(PassiveData passiveData)
        {
            GetComponent<DisplayTooltipOnHover>().SetupPassiveTooltip(passiveData.passiveName, passiveData.description, passiveData.icon);
        }
    }
}
