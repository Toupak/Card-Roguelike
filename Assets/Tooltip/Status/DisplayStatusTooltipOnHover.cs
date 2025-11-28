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
            multiTooltipDisplay = TooltipFactory.instance.CreateTooltip();

            string title = LocalizationSystem.instance.GetStatusTitle(statusTabDisplay.statusData.localizationKey);
            string main = LocalizationSystem.instance.GetStatusDescription(statusTabDisplay.statusData.localizationKey);
            multiTooltipDisplay.SetupTooltip(title, main, statusTabDisplay.cardController.tooltipPivot.position, statusTabDisplay.statusData);
        }
    }
}
