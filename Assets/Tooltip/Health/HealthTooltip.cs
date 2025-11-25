using Localization;
using TMPro;
using UnityEngine;

namespace Tooltip.Health
{
    public class HealthTooltip : TooltipDisplay
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI mainText;
        
        public void SetupHealthTooltip(int health)
        {
            title.text = LocalizationSystem.instance.GetCombatString("health_tooltip_title");
            string text = LocalizationSystem.instance.GetCombatString("health_tooltip");
            text = text.Replace("%h%", $"{health}");
            mainText.text = CheckForIcons(text);
        }
    }
}
