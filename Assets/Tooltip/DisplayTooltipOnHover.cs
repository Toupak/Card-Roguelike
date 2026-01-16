using Cards.Scripts;
using Items;
using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tooltip
{
    public abstract class DisplayTooltipOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private CardController cardController;
        [SerializeField] private ItemController itemController;
        
        [SerializeField] private RectTransform targetToSqueezeOnHover;
        [SerializeField] private float squeezePowerOnHover;

        protected TooltipDisplay tooltipDisplay;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (tooltipDisplay != null || (cardController != null && cardController.cardMovement.isDragging) || (itemController != null && itemController.cardMovement.isDragging))
                return;

            DisplayTooltip();
            
            if (targetToSqueezeOnHover != null && squeezePowerOnHover > 0.0f)
                Tween.PunchScale(targetToSqueezeOnHover, Vector3.down * squeezePowerOnHover, 0.1f);
        }

        protected virtual void DisplayTooltip()
        {
            tooltipDisplay = TooltipFactory.instance.CreateTooltip("base", "base", Vector2.zero);
        }

        private void HideTooltip()
        {
            if (tooltipDisplay != null)
            {
                tooltipDisplay.Hide();
                tooltipDisplay = null;
            }
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            HideTooltip();
        }

        private void OnDisable()
        {
            HideTooltip();
        }

        private void OnDestroy()
        {
            HideTooltip();
        }
    }
}
