using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tooltip
{
    public abstract class DisplayTooltipOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private RectTransform targetToSqueezeOnHover;
        [SerializeField] private float squeezePowerOnHover;

        protected MultiTooltipDisplay multiTooltipDisplay;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (multiTooltipDisplay != null)
                return;

            DisplayTooltip();
            
            if (targetToSqueezeOnHover != null && squeezePowerOnHover > 0.0f)
                Tween.PunchScale(targetToSqueezeOnHover, Vector3.down * squeezePowerOnHover, 0.1f);
        }

        protected virtual void DisplayTooltip()
        {
            multiTooltipDisplay = TooltipFactory.instance.CreateTooltip();
        }

        private void HideTooltip()
        {
            if (multiTooltipDisplay != null)
            {
                multiTooltipDisplay.Hide();
                multiTooltipDisplay = null;
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

        protected string CheckForDamage(string description, int damage)
        {
            return CheckForDamage(description, $"{damage}");
        }
        
        protected string CheckForDamage(string description, string damage)
        {
            return description.Replace("$d$", damage);
        }
    }
}
