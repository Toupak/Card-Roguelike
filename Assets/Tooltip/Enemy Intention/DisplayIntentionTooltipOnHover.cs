using Cards.Scripts;
using Localization;
using UnityEngine;

namespace Tooltip.Enemy_Intention
{
    public class DisplayIntentionTooltipOnHover : DisplayTooltipOnHover
    {
        [SerializeField] private CardController cardController;
        
        protected override void DisplayTooltip()
        {
            if (cardController.enemyCardController == null || !cardController.enemyCardController.hasIntention)
                return;
                
            tooltipDisplay = TooltipFactory.instance.CreateTooltip();
            tooltipDisplay.SetPosition(cardController.tooltipPivot.position);

            string title = LocalizationSystem.instance.GetEnemyBehaviourTitle(cardController.enemyCardController.cardController.cardData.localizationKey, cardController.enemyCardController.nextbehaviour.localizationKey);
            string main = LocalizationSystem.instance.GetEnemyBehaviourDescription(cardController.enemyCardController.cardController.cardData.localizationKey, cardController.enemyCardController.nextbehaviour.localizationKey);

            main = CheckForIcons(main);
            main = CheckForDamage(main, cardController.enemyCardController.nextbehaviour.GetDamageText());
            
            tooltipDisplay.SetupTooltip(title, main);
        }
    }
}
