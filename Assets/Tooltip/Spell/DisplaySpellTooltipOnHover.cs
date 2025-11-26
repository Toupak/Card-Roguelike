using Spells;
using UnityEngine;

namespace Tooltip.Spell
{
    public class DisplaySpellTooltipOnHover : DisplayTooltipOnHover
    {
        [SerializeField] private SpellButton spellButton;
        
        protected override void DisplayTooltip()
        {
            tooltipDisplay = TooltipFactory.instance.CreateSpellTooltip();
            
            tooltipDisplay.SetPosition(spellButton.spellController.cardController.tooltipPivot.position);
            ((SpellTooltip)tooltipDisplay).SetupSpellTooltip(spellButton.spellController);
        }
    }
}
