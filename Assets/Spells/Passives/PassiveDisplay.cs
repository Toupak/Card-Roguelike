using Cards.Scripts;
using Tooltip;
using UnityEngine;
using UnityEngine.UI;

namespace Spells.Passives
{
    public class PassiveDisplay : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image background;

        private PassiveController passiveController;
        private PassiveData data;

        public void Setup(CardController cardController, PassiveData passiveData)
        {
            data = passiveData;
            background.color = passiveData.backgroundColor;
            
            if (passiveData.icon != null)
                icon.sprite = passiveData.icon;
            
            SetupTooltip(passiveData);
            SetupSpellController(cardController, passiveData);
        }
        
        private void SetupSpellController(CardController cardController, PassiveData passiveData)
        {
            if (data.passiveController != null)
            {
                passiveController = Instantiate(data.passiveController, transform);
                passiveController.Setup(cardController, passiveData);
            }
        }
        
        private void SetupTooltip(PassiveData passiveData)
        {
            GetComponent<DisplayTooltipOnHover>().SetTextToDisplay(passiveData.passiveName, passiveData.description, TooltipDisplay.TooltipType.Spell);
        }
    }
}
