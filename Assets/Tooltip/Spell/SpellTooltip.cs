using CombatLoop.EnergyBar;
using Localization;
using Spells;
using TMPro;
using UnityEngine;

namespace Tooltip.Spell
{
    public class SpellTooltip : TooltipDisplay
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI mainText;
        
        [Space]
        [SerializeField] private GameObject energyGameObject;
        [SerializeField] private RectTransform energyBackground;
        [SerializeField] private GameObject filledEnergyPrefab;
        [SerializeField] private GameObject emptyEnergyPrefab;
        [SerializeField] private Transform energyHolder;

        public void SetupSpellTooltip(SpellController spellController)
        {
            title.text = LocalizationSystem.instance.GetSpellTitle(spellController.cardController.cardData.localizationKey, spellController.spellData.localizationKey);
            
            int damage = spellController.ComputeCurrentDamage(spellController.spellData.damage);
            string description = LocalizationSystem.instance.GetSpellDescription(spellController.cardController.cardData.localizationKey, spellController.spellData.localizationKey);
            
            mainText.text = CheckForDamage(CheckForIcons(description), damage);

            AddEnergyCost(spellController.ComputeEnergyCost());
        }

        public void AddEnergyCost(int energyCostToDisplay)
        {
            int maxEnergy = EnergyController.instance != null ? EnergyController.instance.currentMaxEnergy : 3;
            
            energyGameObject.SetActive(true);
            energyBackground.sizeDelta = new Vector2(55.0f, 166.0f + (55.0f * (maxEnergy - 3)));

            for (int i = 0; i < maxEnergy; i++)
            {
                Instantiate(i < energyCostToDisplay ? filledEnergyPrefab : emptyEnergyPrefab, energyHolder);
            }
        }
    }
}
