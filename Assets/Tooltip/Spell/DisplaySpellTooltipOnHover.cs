using Localization;
using Spells;
using UnityEngine;

namespace Tooltip.Spell
{
    public class DisplaySpellTooltipOnHover : DisplayTooltipOnHover
    {
        [SerializeField] private SpellButton spellButton;
        
        protected override void DisplayTooltip()
        {
            multiTooltipDisplay = TooltipFactory.instance.CreateTooltip();

            SpellController spellController = spellButton.spellController;
            
            string title = LocalizationSystem.instance.GetSpellTitle(spellController.cardController.cardData.localizationKey, spellController.spellData.localizationKey);
            
            int damage = spellController.ComputeCurrentDamage(spellController.spellData.damage);
            string description = LocalizationSystem.instance.GetSpellDescription(spellController.cardController.cardData.localizationKey, spellController.spellData.localizationKey);
            description = CheckForDamage(description, damage);

            multiTooltipDisplay.SetupTooltip(title, description, spellButton.spellController.cardController.tooltipPivot.position);
            multiTooltipDisplay.SetupEnergyCost(spellController.ComputeEnergyCost());
        }
    }
}
