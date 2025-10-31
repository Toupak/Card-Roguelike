using System;
using UnityEngine;
using UnityEngine.EventSystems;
using static Tooltip.TooltipDisplay;

namespace Tooltip
{
    public class DisplayTooltipOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] [TextArea] [Tooltip("Can also be set through code")] private string titleToDisplay;
        [SerializeField] [TextArea] [Tooltip("Can also be set through code")] private string textToDisplay;
        [SerializeField] [TextArea] [Tooltip("title is optional when type == Regular")] private TooltipType tooltipType;
        
        private TooltipDisplay tooltipDisplay;

        public void SetTextToDisplay(string title, string main, TooltipType type)
        {
            titleToDisplay = title;
            textToDisplay = main;
            tooltipType = type;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (tooltipDisplay != null)
                return;

            tooltipDisplay = TooltipFactory.instance.CreateTooltip();
            tooltipDisplay.Setup(titleToDisplay, textToDisplay, tooltipType);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (tooltipDisplay != null)
            {
                tooltipDisplay.Hide();
                tooltipDisplay = null;
            }
        }
    }
}
