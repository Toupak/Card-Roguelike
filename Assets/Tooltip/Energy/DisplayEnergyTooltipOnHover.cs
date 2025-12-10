using CombatLoop.EnergyBar;
using Localization;

namespace Tooltip.Energy
{
    public class DisplayEnergyTooltipOnHover : DisplayTooltipOnHover
    {
        
        protected override void DisplayTooltip()
        {
            int maxEnergy = EnergyController.instance.currentMaxEnergy;
            int currentEnergy = EnergyController.instance.currentEnergy;
            
            string title = LocalizationSystem.instance.GetCombatString("energy_tooltip_title");
            string text = LocalizationSystem.instance.GetCombatString("energy_tooltip");
            text = text.Replace("$e$", $"{currentEnergy}");
            text = text.Replace("$m$", $"{maxEnergy}");
            
            tooltipDisplay = TooltipFactory.instance.CreateTooltip(title, text, transform.parent.GetComponent<EnergyDisplay>().tooltipPivot.position);
        }
    }
}
