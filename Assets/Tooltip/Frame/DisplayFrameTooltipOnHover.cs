using Items;
using UnityEngine;

namespace Tooltip.Frame
{
    public class DisplayFrameTooltipOnHover : DisplayTooltipOnHover
    {
        [SerializeField] private Transform tooltipPivot;
        [SerializeField] private FrameItem frameItem;
        
        protected override void DisplayTooltip()
        {
            string title = frameItem.data.frameName;
            string text = frameItem.data.frameDescription;
            
            tooltipDisplay = TooltipFactory.instance.CreateTooltip(title, text, tooltipPivot.position);
        }
    }
}
