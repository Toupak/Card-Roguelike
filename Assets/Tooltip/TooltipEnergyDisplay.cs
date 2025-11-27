using CombatLoop.EnergyBar;
using UnityEngine;

namespace Tooltip
{
    public class TooltipEnergyDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject energyGameObject;
        [SerializeField] private RectTransform energyBackground;
        [SerializeField] private GameObject filledEnergyPrefab;
        [SerializeField] private GameObject emptyEnergyPrefab;
        [SerializeField] private Transform energyHolder;
    
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
