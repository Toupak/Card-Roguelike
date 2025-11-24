using Passives;
using UnityEngine;

namespace Tooltip.Passive
{
    public class DisplayPassiveTooltipOnHover : DisplayTooltipOnHover
    {
        [SerializeField] private PassiveDisplay passiveDisplay;
        
        protected override void DisplayTooltip()
        {
            tooltipDisplay = TooltipFactory.instance.CreatePassiveTooltip();
            tooltipDisplay.SetPosition(passiveDisplay.cardController.tooltipPivot.position);
            ((PassiveTooltip)tooltipDisplay).SetupPassiveTooltip(passiveDisplay.passiveController);
        }
    }
}
