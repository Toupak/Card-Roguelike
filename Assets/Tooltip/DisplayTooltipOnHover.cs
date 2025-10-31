using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tooltip
{
    public class DisplayTooltipOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] [TextArea] [Tooltip("Can also be set through code")] private string textToDisplay;
        
        private TooltipDisplay tooltipDisplay;

        public void SetTextToDisplay(string newText)
        {
            textToDisplay = newText;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (tooltipDisplay != null)
                return;

            tooltipDisplay = TooltipFactory.instance.CreateTooltip();
            tooltipDisplay.Setup(textToDisplay);
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
