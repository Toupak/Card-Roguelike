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
                
            multiTooltipDisplay = TooltipFactory.instance.CreateTooltip();

            string title = LocalizationSystem.instance.GetEnemyBehaviourTitle(cardController.enemyCardController.cardController.cardData.localizationKey, cardController.enemyCardController.nextbehaviour.localizationKey);
            string main = LocalizationSystem.instance.GetEnemyBehaviourDescription(cardController.enemyCardController.cardController.cardData.localizationKey, cardController.enemyCardController.nextbehaviour.localizationKey);

            main = CheckForDamage(main, cardController.enemyCardController.nextbehaviour.GetDamageText());
            
            multiTooltipDisplay.SetupTooltip(title, main, cardController.tooltipPivot.position);
        }
    }
}
