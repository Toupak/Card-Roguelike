using System.Collections.Generic;
using System.Linq;
using Localization;
using Localization.Icons_In_Text;
using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tooltip
{
    public abstract class DisplayTooltipOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private RectTransform targetToSqueezeOnHover;
        [SerializeField] private float squeezePowerOnHover;

        protected TooltipDisplay tooltipDisplay;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (tooltipDisplay != null)
                return;

            DisplayTooltip();
            
            if (targetToSqueezeOnHover != null && squeezePowerOnHover > 0.0f)
                Tween.PunchScale(targetToSqueezeOnHover, Vector3.down * squeezePowerOnHover, 0.1f);
        }

        protected virtual void DisplayTooltip()
        {
            tooltipDisplay = TooltipFactory.instance.CreateTooltip();
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
        
        protected string CheckForIcons(string message)
        {
            string finalString = message;
            string[] chunks = message.Split('$');

            List<TextToIconRule> rules = LocalizationSystem.instance.textToIconData.rules;
            
            foreach (string chunk in chunks)
            {
                if (rules.Where((r) => r.tag.Equals(chunk)).ToList().Count > 0)
                {
                    TextToIconRule rule = rules.First((r) => r.tag.Equals(chunk));
                    finalString = finalString.Replace($"${rule.tag}$", $"<sprite name=\"{rule.icon.name}\">");
                }
            }
            
            return finalString;
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
