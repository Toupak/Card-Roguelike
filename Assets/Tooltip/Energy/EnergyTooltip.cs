using CombatLoop.EnergyBar;
using TMPro;
using UnityEngine;

namespace Tooltip.Energy
{
    public class EnergyTooltip : TooltipDisplay
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI mainText;
        
        public override void Setup()
        {
            base.Setup();

            int maxEnergy = EnergyController.instance.currentMaxEnergy;
            int currentEnergy = EnergyController.instance.currentEnergy;
            
            title.text = "Energy";
            mainText.text = $"Your current energy, each $energy$ is worth 1 energy.\nYou currently have {currentEnergy}/{maxEnergy} energy";
        }
    }
}
