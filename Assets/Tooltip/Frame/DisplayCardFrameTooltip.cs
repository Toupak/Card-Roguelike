using Items;
using Items.Frames;
using UnityEngine;

namespace Tooltip.Frame
{
    public class DisplayCardFrameTooltip : DisplayTooltipOnHover
    {
        [SerializeField] private FrameTabDisplay frameTabDisplay;
        
        protected override void DisplayTooltip()
        {
            string title = frameTabDisplay.data.frameName;
            string text = frameTabDisplay.data.frameDescription;
            
            tooltipDisplay = TooltipFactory.instance.CreateTooltip(title, text, frameTabDisplay.cardController.tooltipPivot.position);
        }
    }
}
