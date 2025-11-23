using Spells;
using UnityEngine;

namespace Tooltip.Spell
{
    public class DisplaySpellTooltipOnHover : DisplayTooltipOnHover
    {
        [SerializeField] private Transform tooltipPivot;
        [SerializeField] private SpellButton spellButton;
        
        protected override void DisplayTooltip()
        {
            tooltipDisplay = TooltipFactory.instance.CreateSpellTooltip();
            tooltipDisplay.SetPosition(tooltipPivot.position);
            ((SpellTooltip)tooltipDisplay).SetupSpellTooltip(spellButton.spellController);
        }
    }
}
