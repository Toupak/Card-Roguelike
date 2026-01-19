using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Inventory.Drop_Rates
{
    internal class DropRateCalculator
    {
        public readonly DropRateData dropRateData;
        public float currentChance;
        
        public DropRateCalculator(DropRateData dropRateData)
        {
            this.dropRateData = dropRateData;
            currentChance = dropRateData.startingValue;
        }

        public bool Check()
        {
            bool result = Random.Range(0.0f, 100.0f) <= currentChance;

            if (result)
            {
                currentChance = dropRateData.startingValue;
                Debug.Log($"Success, reset value to : {currentChance}");
            }
            else
            {
                currentChance += dropRateData.valueIncreaseOnFail;
                Debug.Log($"Fail, new value : {currentChance}");
            }

            return result;
        }

        public void BoostChancesOnEliteKill()
        {
            currentChance += dropRateData.valueIncreaseOnEliteKill;
        }
        
        public void BoostChancesOnBossKill()
        {
            currentChance += dropRateData.valueIncreaseOnBossKill;
        }
    }
    
    public class DropRateManager : MonoBehaviour
    {
        [SerializeField] private DropRateData cardsLegendary;
        [SerializeField] private DropRateData cardsExotic;
        
        [Space]
        [SerializeField] private DropRateData frame;
        [SerializeField] private DropRateData framesLegendary;
        [SerializeField] private DropRateData framesExotic;
        
        public static DropRateManager instance;

        private DropRateCalculator cardsLegendaryCalculator;
        private DropRateCalculator cardsExoticCalculator;
        
        private DropRateCalculator frameCalculator;
        private DropRateCalculator frameLegendaryCalculator;
        private DropRateCalculator frameExoticCalculator;
        
        
        private void Awake()
        {
            instance = this;

            cardsLegendaryCalculator = new DropRateCalculator(cardsLegendary);
            cardsExoticCalculator = new DropRateCalculator(cardsExotic);
            
            frameCalculator = new DropRateCalculator(frame);
            frameLegendaryCalculator = new DropRateCalculator(framesLegendary);
            frameExoticCalculator = new DropRateCalculator(framesExotic);
        }

        public bool CheckForFrameReward()
        {
            Debug.Log("Checking for Frame drop...");
            return frameCalculator.Check();
        }
        
        public bool CheckForLegendaryFrameReward()
        {
            Debug.Log("Checking for Legendary frame drop...");
            return frameLegendaryCalculator.Check();
        }

        public bool CheckForExoticFrameReward()
        {
            Debug.Log("Checking for Exotic frame drop...");
            return frameExoticCalculator.Check();
        }

        public bool CheckForLegendaryCardReward()
        {
            Debug.Log("Checking for Legendary card drop...");
            return cardsLegendaryCalculator.Check();
        }

        public bool CheckForExoticCardReward()
        {
            Debug.Log("Checking for Exotic card drop...");
            return cardsExoticCalculator.Check();
        }

        public void BoostChancesOnEliteKill()
        {
            cardsLegendaryCalculator.BoostChancesOnEliteKill();
            cardsExoticCalculator.BoostChancesOnEliteKill();
        }
        
        public void BoostChancesOnBossKill()
        {
            cardsLegendaryCalculator.BoostChancesOnBossKill();
            cardsExoticCalculator.BoostChancesOnBossKill();
        }
    }
}
