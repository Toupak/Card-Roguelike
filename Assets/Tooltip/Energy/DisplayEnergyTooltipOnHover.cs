using UnityEngine;

namespace Tooltip.Energy
{
    public class DisplayEnergyTooltipOnHover : DisplayTooltipOnHover
    {
        protected override void DisplayTooltip()
        {
            base.DisplayTooltip();
            tooltipDisplay.SetPosition(transform.parent.position, false);
        }
    }
}
