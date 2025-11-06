using System;
using UnityEngine;

namespace Run_Loop.Run_Parameters
{
    public class RunParameterData
    {
        public int boosterCount { get; private set; }
        public int cardCount { get; private set; }
        public int fightCount { get; private set; }

        public RunParameterData(int booster, int card, int fight)
        {
            boosterCount = booster;
            cardCount = card;
            fightCount = fight;
        }
    }
    
    public class RunParameterGatherer : MonoBehaviour
    {
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
            
            selectedRunParameter = new RunParameterData(boosterCount.currentValue, cardCount.currentValue, fightCount.currentValue);
        }
    }
}
