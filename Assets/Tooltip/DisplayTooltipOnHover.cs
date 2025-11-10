using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;
using static Tooltip.TooltipDisplay;

namespace Tooltip
{
    public class DisplayTooltipOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private string titleToDisplay;
        [SerializeField] [TextArea] private string textToDisplay;
        [SerializeField] private TooltipType tooltipType;
        [SerializeField] private int energyCostToDisplay;
        [SerializeField] private Sprite iconToDisplay;
        
        [Space]
        [SerializeField] private RectTransform targetToSqueezeOnHover;
        [SerializeField] private float squeezePowerOnHover;

        [Space] 
        [SerializeField] private bool displayOnTheLeft;
        
        private TooltipDisplay tooltipDisplay;

        public void SetupSpellTooltip(string title, string main, int energyCost, Sprite icon)
        {
            titleToDisplay = title;
            textToDisplay = main;
            energyCostToDisplay = energyCost;
            iconToDisplay = icon;
            tooltipType = TooltipType.Spell;
        }
        
        public void SetupPassiveTooltip(string title, string main, Sprite icon)
        {
            titleToDisplay = title;
            textToDisplay = main;
            iconToDisplay = icon;
            tooltipType = TooltipType.Passive;
        }
        
        public void SetupEnemyIntentionTooltip(string title, string main, Sprite icon)
        {
            titleToDisplay = title;
            textToDisplay = main;
            iconToDisplay = icon;
            tooltipType = TooltipType.Passive;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (tooltipDisplay != null)
                return;

            DisplayTooltip();
            
            if (targetToSqueezeOnHover != null && squeezePowerOnHover > 0.0f)
                Tween.PunchScale(targetToSqueezeOnHover, Vector3.down * squeezePowerOnHover, 0.1f);
        }

        private void DisplayTooltip()
        {
            switch (tooltipType)
            {
                case TooltipType.Spell:
                    tooltipDisplay = TooltipFactory.instance.CreateTooltip()
                        .AddTitle(titleToDisplay)
                        .AddMainText(textToDisplay)
                        .SetDisplaySide(displayOnTheLeft)
                        .AddEnergyCost(energyCostToDisplay)
                        .AddIcon(iconToDisplay);
                    break;
                case TooltipType.EnemyIntentions:
                case TooltipType.Passive:
                    tooltipDisplay = TooltipFactory.instance.CreateTooltip()
                        .AddTitle(titleToDisplay)
                        .AddMainText(textToDisplay)
                        .SetDisplaySide(displayOnTheLeft)
                        .AddIcon(iconToDisplay);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
