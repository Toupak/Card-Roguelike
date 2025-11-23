using Tooltip.Energy;
using UnityEngine;

namespace Tooltip
{
    public class TooltipFactory : MonoBehaviour
    {
        [SerializeField] private EnergyTooltip energyTooltipPrefab;
        
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
    }
}
