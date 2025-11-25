using Cards.Scripts;
using Localization;
using Passives;
using TMPro;
using UnityEngine;

namespace Tooltip.Passive
{
    public class PassiveTooltip : TooltipDisplay
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI mainText;
        
        public void SetupPassiveTooltip(PassiveController passiveController)
        {
            title.text = passiveController.passiveData.passiveName;
            string description = LocalizationSystem.instance.GetPassiveDescription(passiveController.cardController.cardData.localizationKey, passiveController.passiveData.localizationKey);
            
            mainText.text = CheckForIcons(description);
        }
    }
}
