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
            title.text = "Health";
            mainText.text = ComputeMainText(health);
        }

        private string ComputeMainText(int health)
        {
            string text = $"This card's health\nThe card dies when its health reaches 0\nHealth is not refilled after a battle\nThis card has %h% health";
            text = text.Replace("%h%", $"{health}");

            return text;
        }
    }
}
