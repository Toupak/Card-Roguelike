using Tooltip.Energy;
using Tooltip.Health;
using Tooltip.Spell;
using UnityEngine;

namespace Tooltip
{
    public class TooltipFactory : MonoBehaviour
    {
        [SerializeField] private EnergyTooltip energyTooltipPrefab;
        [SerializeField] private HealthTooltip healthTooltipPrefab;
        [SerializeField] private SpellTooltip spellTooltipPrefab;
        
        public static TooltipFactory instance;

        private void Awake()
        {
            instance = this;
        }

        public EnergyTooltip CreateEnergyTooltip()
        {
            EnergyTooltip energyTooltip = Instantiate(energyTooltipPrefab, transform);
            energyTooltip.Setup();
            return energyTooltip;
        }
        
        public HealthTooltip CreateHealthTooltip()
        {
            HealthTooltip healthTooltip = Instantiate(healthTooltipPrefab, transform);
            healthTooltip.Setup();
            return healthTooltip;
        }
        
        public SpellTooltip CreateSpellTooltip()
        {
            SpellTooltip spellTooltip = Instantiate(spellTooltipPrefab, transform);
            spellTooltip.Setup();
            return spellTooltip;
        }
    }
}
