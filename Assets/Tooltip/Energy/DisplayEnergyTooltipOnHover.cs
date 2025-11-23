namespace Tooltip.Energy
{
    public class DisplayEnergyTooltipOnHover : DisplayTooltipOnHover
    {
        protected override void DisplayTooltip()
        {
            tooltipDisplay = TooltipFactory.instance.CreateEnergyTooltip();
            tooltipDisplay.SetPosition(transform.parent.position);
        }
    }
}
