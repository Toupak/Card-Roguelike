using Items;
using UnityEngine;

namespace Tooltip.Frame
{
    public class DisplayFrameTooltipOnHover : DisplayTooltipOnHover
    {
        [SerializeField] private Transform tooltipPivot;
        [SerializeField] private FrameCardItem frameCardItem;
        
        protected override void DisplayTooltip()
        {
            string title = frameCardItem.data.frameName;
            string text = frameCardItem.data.frameDescription;
            
            tooltipDisplay = TooltipFactory.instance.CreateTooltip(title, text, tooltipPivot.position);
        }
    }
}
