using Combat.Spells;
using UnityEngine;

namespace Tooltip.Spell
{
    public class DisplaySpellTooltipOnHover : DisplayTooltipOnHover
    {
        [SerializeField] private SpellButton spellButton;
        
        protected override void DisplayTooltip()
        {
            SpellController spellController = spellButton.spellController;

            string title = spellController.ComputeTooltipTitle();
            string description = spellController.ComputeTooltipDescription();

            tooltipDisplay = TooltipFactory.instance.CreateTooltip(title, description, spellButton.spellController.cardController.tooltipPivot.position, energyCost:spellController.ComputeEnergyCost());
        }
    }
}
