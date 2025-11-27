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
            tooltipDisplay = TooltipFactory.instance.CreateTooltip();
            tooltipDisplay.SetPosition(spellButton.spellController.cardController.tooltipPivot.position);

            SpellController spellController = spellButton.spellController;
            
            string title = LocalizationSystem.instance.GetSpellTitle(spellController.cardController.cardData.localizationKey, spellController.spellData.localizationKey);
            
            int damage = spellController.ComputeCurrentDamage(spellController.spellData.damage);
            string description = LocalizationSystem.instance.GetSpellDescription(spellController.cardController.cardData.localizationKey, spellController.spellData.localizationKey);
            description = CheckForDamage(CheckForIcons(description), damage);

            tooltipDisplay.GetComponent<TooltipEnergyDisplay>().AddEnergyCost(spellController.ComputeEnergyCost());
            tooltipDisplay.SetupTooltip(title, description);
        }
    }
}
