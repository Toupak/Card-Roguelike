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
            string title = LocalizationSystem.instance.GetCombatString("health_tooltip_title");
            string text = LocalizationSystem.instance.GetCombatString("health_tooltip");
            text = text.Replace("%h%", $"{cardHealth.currentHealth}");
            
            tooltipDisplay = TooltipFactory.instance.CreateTooltip(title, text, tooltipPivot.position);
        }
    }
}
