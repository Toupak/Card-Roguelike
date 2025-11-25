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
            if (passiveController.cardController.cardData.isEnemy)
            {
                title.text = LocalizationSystem.instance.GetEnemyPassiveTitle(passiveController.cardController.cardData.localizationKey, passiveController.passiveData.localizationKey);
                string description = LocalizationSystem.instance.GetEnemyPassiveDescription(passiveController.cardController.cardData.localizationKey, passiveController.passiveData.localizationKey);

                mainText.text = CheckForIcons(description);
            }
            else
            {
                title.text = LocalizationSystem.instance.GetPassiveTitle(passiveController.cardController.cardData.localizationKey, passiveController.passiveData.localizationKey);
                string description = LocalizationSystem.instance.GetPassiveDescription(passiveController.cardController.cardData.localizationKey, passiveController.passiveData.localizationKey);

                mainText.text = CheckForIcons(description);
            }
        }
    }
}
