using Combat.EnemyAttack;

namespace Tooltip.Enemy_Intention
{
    public class DisplayIntentionTooltipOnHover : DisplayTooltipOnHover
    {
        protected override void DisplayTooltip()
        {
            if (cardController.enemyCardController == null || !cardController.enemyCardController.hasIntention)
                return;

            BaseEnemyBehaviour nextBehaviour = cardController.enemyCardController.nextbehaviour;

            string title = nextBehaviour.ComputeTooltipTitle();
            string main = nextBehaviour.ComputeTooltipDescription();
            
            tooltipDisplay = TooltipFactory.instance.CreateTooltip(title, main, cardController.tooltipPivot.position);
        }
    }
}
