using CombatLoop.EnergyBar;
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
            title.text = spellController.spellData.spellName;
            mainText.text = spellController.spellData.description;

            AddEnergyCost(spellController.ComputeEnergyCost());
        }
        
        public void AddEnergyCost(int energyCostToDisplay)
        {
            int maxEnergy = EnergyController.instance != null ? EnergyController.instance.currentMaxEnergy : 3;
            
            energyGameObject.SetActive(true);
            energyBackground.sizeDelta = new Vector2(45.0f, 118.0f + (35.0f * (maxEnergy - 3)));

            for (int i = 0; i < maxEnergy; i++)
            {
                Instantiate(i < energyCostToDisplay ? filledEnergyPrefab : emptyEnergyPrefab, energyHolder);
            }
        }
    }
}
