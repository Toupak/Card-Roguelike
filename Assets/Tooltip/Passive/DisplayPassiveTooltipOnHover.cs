using Localization;
using Passives;
using UnityEngine;

namespace Tooltip.Passive
{
    public class DisplayPassiveTooltipOnHover : DisplayTooltipOnHover
    {
        [SerializeField] private PassiveDisplay passiveDisplay;
        
        protected override void DisplayTooltip()
        {
            multiTooltipDisplay = TooltipFactory.instance.CreateTooltip();

            PassiveController passiveController = passiveDisplay.passiveController;
            string title;
            string description;
            
            if (passiveController.cardController.cardData.isEnemy)
            {
                title = LocalizationSystem.instance.GetEnemyPassiveTitle(passiveController.cardController.cardData.localizationKey, passiveController.passiveData.localizationKey);
                description = LocalizationSystem.instance.GetEnemyPassiveDescription(passiveController.cardController.cardData.localizationKey, passiveController.passiveData.localizationKey);
            }
            else
            {
                title = LocalizationSystem.instance.GetPassiveTitle(passiveController.cardController.cardData.localizationKey, passiveController.passiveData.localizationKey);
                description = LocalizationSystem.instance.GetPassiveDescription(passiveController.cardController.cardData.localizationKey, passiveController.passiveData.localizationKey);
            }
            
            multiTooltipDisplay.SetupTooltip(title, description, passiveDisplay.cardController.tooltipPivot.position);
        }
    }
}
