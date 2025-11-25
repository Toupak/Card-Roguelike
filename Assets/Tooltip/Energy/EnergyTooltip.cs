using CombatLoop.EnergyBar;
using Localization;
using TMPro;
using UnityEngine;

namespace Tooltip.Energy
{
    public class EnergyTooltip : TooltipDisplay
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI mainText;
        
        public override void Setup()
        {
            base.Setup();

            int maxEnergy = EnergyController.instance.currentMaxEnergy;
            int currentEnergy = EnergyController.instance.currentEnergy;
            
            title.text = LocalizationSystem.instance.GetCombatString("energy_tooltip_title");
            string text = LocalizationSystem.instance.GetCombatString("energy_tooltip");
            text = text.Replace("%e%", $"{currentEnergy}");
            text = text.Replace("%m%", $"{maxEnergy}");
            mainText.text = CheckForIcons(text);
        }
    }
}
