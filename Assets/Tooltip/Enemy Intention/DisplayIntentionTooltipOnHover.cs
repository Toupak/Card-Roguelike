using Cards.Scripts;
using EnemyAttack;
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

            BaseEnemyBehaviour nextBehaviour = cardController.enemyCardController.nextbehaviour;

            string title = nextBehaviour.ComputeTooltipTitle();
            string main = nextBehaviour.ComputeTooltipDescription();
            
            tooltipDisplay = TooltipFactory.instance.CreateTooltip(title, main, cardController.tooltipPivot.position);
        }
    }
}
