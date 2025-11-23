using Cards.Scripts;
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
                
            tooltipDisplay = TooltipFactory.instance.CreateEnemyIntentionTooltip();
            tooltipDisplay.SetPosition(cardController.tooltipPivot.position);
            ((IntentionTooltip)tooltipDisplay).SetupIntentionTooltip(cardController.enemyCardController);
        }
    }
}
