using Cards.Scripts;
using Inventory.Items;
using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tooltip
{
    public abstract class DisplayTooltipOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected CardController cardController;
        [SerializeField] protected ItemController itemController;
        
        [SerializeField] private RectTransform targetToSqueezeOnHover;
        [SerializeField] private float squeezePowerOnHover;

        protected TooltipDisplay tooltipDisplay;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!CanTooltipBeDisplayed())
                return;

            DisplayTooltip();
            
            if (targetToSqueezeOnHover != null && squeezePowerOnHover > 0.0f)
                Tween.PunchScale(targetToSqueezeOnHover, Vector3.down * squeezePowerOnHover, 0.1f);
        }

        protected virtual bool CanTooltipBeDisplayed()
        {
            return (tooltipDisplay == null && (cardController == null || !cardController.cardMovement.isDragging) && (itemController == null || !itemController.cardMovement.isDragging));
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
