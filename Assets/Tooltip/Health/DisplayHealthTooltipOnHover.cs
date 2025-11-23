using Cards.Scripts;
using UnityEngine;

namespace Tooltip.Health
{
    public class DisplayHealthTooltipOnHover : DisplayTooltipOnHover
    {
        [SerializeField] private Transform tooltipPivot;
        [SerializeField] private CardHealth cardHealth;
        
        protected override void DisplayTooltip()
        {
            tooltipDisplay = TooltipFactory.instance.CreateHealthTooltip();
            tooltipDisplay.SetPosition(tooltipPivot.position);
            ((HealthTooltip)tooltipDisplay).SetupHealthTooltip(cardHealth.currentHealth);
        }
    }
}
