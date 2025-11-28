using UnityEngine;

namespace Run_Loop.Run_Parameters
{
    public class RunParameterData
    {
        public int startBoosterCount { get; private set; }
        public int startCardCount { get; private set; }
        public int boosterCount { get; private set; }
        public int cardCount { get; private set; }
        public int fightCount { get; private set; }

        public RunParameterData(int startBooster, int startCard, int booster, int card, int fight)
        {
            startBoosterCount = startBooster;
            startCardCount = startCard;
            boosterCount = booster;
            cardCount = card;
            fightCount = fight;
        }
    }
    
    public class RunParameterGatherer : MonoBehaviour
    {
        [SerializeField] private OptionDisplay startBoosterCount;
        [SerializeField] private OptionDisplay startCardCount;
        [SerializeField] private OptionDisplay boosterCount;
        [SerializeField] private OptionDisplay cardCount;
        [SerializeField] private OptionDisplay fightCount;

        public static RunParameterGatherer instance;

        public RunParameterData selectedRunParameter = null;
        public bool isParameterSelected => selectedRunParameter != null;
        
        private void Awake()
        {
            instance = this;
        }

        public void ValidateParameters()
        {
            if (isParameterSelected)
                return;
            
            selectedRunParameter = new RunParameterData(startBoosterCount.currentValue, startCardCount.currentValue, boosterCount.currentValue, cardCount.currentValue, fightCount.currentValue);
        }
    }
}
