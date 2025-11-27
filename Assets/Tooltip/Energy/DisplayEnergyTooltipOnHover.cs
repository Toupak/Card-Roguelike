using CombatLoop.EnergyBar;
using Localization;

namespace Tooltip.Energy
{
    public class DisplayEnergyTooltipOnHover : DisplayTooltipOnHover
    {
        
        protected override void DisplayTooltip()
        {
            tooltipDisplay = TooltipFactory.instance.CreateTooltip();
            tooltipDisplay.SetPosition(transform.parent.GetComponent<EnergyDisplay>().tooltipPivot.position);
            
            int maxEnergy = EnergyController.instance.currentMaxEnergy;
            int currentEnergy = EnergyController.instance.currentEnergy;
            
            string title = LocalizationSystem.instance.GetCombatString("energy_tooltip_title");
            string text = LocalizationSystem.instance.GetCombatString("energy_tooltip");
            text = text.Replace("%e%", $"{currentEnergy}");
            text = text.Replace("%m%", $"{maxEnergy}");
            text = CheckForIcons(text);
            
            tooltipDisplay.SetupTooltip(title, text);
        }
    }
}
