using Cards.Scripts;
using Combat.Spells;
using Localization;
using Tooltip;
using UnityEngine;

namespace Map.Encounters.Fusion.Spell_Button_Toggle
{
    public class DisplaySpellToggleTooltipOnHover : DisplayTooltipOnHover
    {
        [SerializeField] private Transform tooltipPivot;
        
        private string ownerLocalizationKey;
        public SpellData currentSpellData { get; private set; }

        private bool isSetup;

        public void Setup(CardController originalSpellOwner, SpellData spellData)
        {
            ownerLocalizationKey = originalSpellOwner.cardData.localizationKey;
            currentSpellData = spellData;
            
            isSetup = true;
        }
        
        protected override bool CanTooltipBeDisplayed()
        {
            return isSetup;
        }
        
        protected override void DisplayTooltip()
        {
            string title = ComputeTooltipTitle();
            string description = ComputeTooltipDescription();

            tooltipDisplay = TooltipFactory.instance.CreateTooltip(title, description, tooltipPivot.position, energyCost:currentSpellData.energyCost);
        }

        private string ComputeTooltipTitle()
        {
            return LocalizationSystem.instance.GetSpellTitle(ownerLocalizationKey, currentSpellData.localizationKey);
        }

        private string ComputeTooltipDescription()
        {
            string description = LocalizationSystem.instance.GetSpellDescription(ownerLocalizationKey, currentSpellData.localizationKey);
            description = LocalizationSystem.instance.CheckForDamageInText(description, currentSpellData.damage.ToString(), LocalizationSystem.TextDisplayStyle.None);

            return description;
        }
    }
}
