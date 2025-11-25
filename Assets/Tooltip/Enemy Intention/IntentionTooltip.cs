using EnemyAttack;
using Localization;
using TMPro;
using UnityEngine;

namespace Tooltip.Enemy_Intention
{
    public class IntentionTooltip : TooltipDisplay
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI mainText;
        
        public void SetupIntentionTooltip(EnemyCardController enemyCardController)
        {
            title.text = LocalizationSystem.instance.GetEnemyBehaviourTitle(enemyCardController.cardController.cardData.localizationKey, enemyCardController.nextbehaviour.localizationKey);
            string description = LocalizationSystem.instance.GetEnemyBehaviourDescription(enemyCardController.cardController.cardData.localizationKey, enemyCardController.nextbehaviour.localizationKey);

            mainText.text = CheckForDamage(CheckForIcons(description), enemyCardController.nextbehaviour.GetDamageText());
        }
    }
}
