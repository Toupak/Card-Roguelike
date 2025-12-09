using Passives;
using UnityEngine;

namespace Tooltip.Passive
{
    public class DisplayPassiveTooltipOnHover : DisplayTooltipOnHover
    {
        [SerializeField] private PassiveDisplay passiveDisplay;
        
        protected override void DisplayTooltip()
        {
            PassiveController passiveController = passiveDisplay.passiveController;
         
            string title = passiveController.ComputeTooltipTitle();
            string description = passiveController.ComputeTooltipDescription();

            multiTooltipDisplay = TooltipFactory.instance.CreateTooltip();
            multiTooltipDisplay.SetupTooltip(title, description, passiveDisplay.cardController.tooltipPivot.position);
        }
    }
}
