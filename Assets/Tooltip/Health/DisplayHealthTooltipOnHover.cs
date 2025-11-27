using Cards.Scripts;
using Localization;
using UnityEngine;

namespace Tooltip.Health
{
    public class DisplayHealthTooltipOnHover : DisplayTooltipOnHover
    {
        [SerializeField] private Transform tooltipPivot;
        [SerializeField] private CardHealth cardHealth;
        
        protected override void DisplayTooltip()
        {
            tooltipDisplay = TooltipFactory.instance.CreateTooltip();
            tooltipDisplay.SetPosition(tooltipPivot.position);
            
            string title = LocalizationSystem.instance.GetCombatString("health_tooltip_title");
            string text = LocalizationSystem.instance.GetCombatString("health_tooltip");
            text = text.Replace("%h%", $"{cardHealth.currentHealth}");
            text = CheckForIcons(text);
            
            tooltipDisplay.SetupTooltip(title, text);
        }
    }
}
