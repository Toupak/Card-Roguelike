using Status;
using UnityEngine;

namespace Tooltip.Status
{
    public class DisplayStatusTooltipOnHover : DisplayTooltipOnHover
    {
        [SerializeField] private StatusTabDisplay statusTabDisplay;
        
        protected override void DisplayTooltip()
        {
            tooltipDisplay = TooltipFactory.instance.CreateStatusTooltip();
            tooltipDisplay.SetPosition(statusTabDisplay.cardController.tooltipPivot.position);
            ((StatusTooltip)tooltipDisplay).SetupStatusTooltip(statusTabDisplay.statusData);
        }
    }
}
