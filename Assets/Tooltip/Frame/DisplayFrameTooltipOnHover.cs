using Items;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tooltip.Frame
{
    public class DisplayFrameTooltipOnHover : DisplayTooltipOnHover
    {
        [SerializeField] private Transform tooltipPivot;
        [FormerlySerializedAs("frameItem")] [SerializeField] private FrameCardItem frameCardItem;
        
        protected override void DisplayTooltip()
        {
            string title = frameCardItem.data.frameName;
            string text = frameCardItem.data.frameDescription;
            
            tooltipDisplay = TooltipFactory.instance.CreateTooltip(title, text, tooltipPivot.position);
        }
    }
}
