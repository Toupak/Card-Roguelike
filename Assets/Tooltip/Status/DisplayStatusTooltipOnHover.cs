using Localization;
using Status;
using UnityEngine;

namespace Tooltip.Status
{
    public class DisplayStatusTooltipOnHover : DisplayTooltipOnHover
    {
        [SerializeField] private StatusTabDisplay statusTabDisplay;
        
        protected override void DisplayTooltip()
        {
            tooltipDisplay = TooltipFactory.instance.CreateTooltip();
            tooltipDisplay.SetPosition(statusTabDisplay.cardController.tooltipPivot.position);

            string title = LocalizationSystem.instance.GetStatusTitle(statusTabDisplay.statusData.localizationKey);
            string main = LocalizationSystem.instance.GetStatusDescription(statusTabDisplay.statusData.localizationKey);
            tooltipDisplay.SetupTooltip(title, main);
        }
    }
}
