using Cards.Scripts;
using Combat.Passives;
using Combat.Spells;
using Localization;
using Tooltip;
using UnityEngine;
using UnityEngine.UI;

namespace Map.Encounters.Fusion.Spell_Button_Toggle
{
    public class DisplayToggleTooltipOnHover : DisplayTooltipOnHover
    {
        [SerializeField] private Transform tooltipPivot;
        
        public SpellData currentSpellData { get; private set; }
        public PassiveData currentPassiveData { get; private set; }

        private string ownerLocalizationKey;
        private string localizationKey;
        private int damage;
        private int energyCost;
        
        private bool isSetup;

        public void Setup(CardController originalSpellOwner, SpellData spellData)
        {
            ownerLocalizationKey = originalSpellOwner.cardData.localizationKey;
            currentSpellData = spellData;

            localizationKey = currentSpellData.localizationKey;
            damage = currentSpellData.damage;
            energyCost = currentSpellData.energyCost;
            
            isSetup = true;
        }
        
        public void Setup(CardController originalPassiveOwner, PassiveData passiveData)
        {
            ownerLocalizationKey = originalPassiveOwner.cardData.localizationKey;
            currentPassiveData = passiveData;
            
            localizationKey = currentPassiveData.localizationKey;
            damage = 0;
            energyCost = -1;

            GetComponent<Image>().color = passiveData.backgroundColor;
            
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

            tooltipDisplay = TooltipFactory.instance.CreateTooltip(title, description, tooltipPivot.position, energyCost:energyCost);
        }

        private string ComputeTooltipTitle()
        {
            return LocalizationSystem.instance.GetSpellTitle(ownerLocalizationKey, localizationKey);
        }

        private string ComputeTooltipDescription()
        {
            string description = LocalizationSystem.instance.GetSpellDescription(ownerLocalizationKey, localizationKey);
            description = LocalizationSystem.instance.CheckForDamageInText(description, damage.ToString(), LocalizationSystem.TextDisplayStyle.None);

            return description;
        }
    }
}
