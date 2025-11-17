using System;
using System.Collections;
using ActionReaction;
using ActionReaction.Game_Actions;
using UnityEngine;
using UnityEngine.Events;

namespace CombatLoop.EnergyBar
{
    public class EnergyController : MonoBehaviour
    {
        public static EnergyController instance;

        [HideInInspector] public static UnityEvent OnOutOfEnergy = new UnityEvent();
        [HideInInspector] public static UnityEvent<int> OnInitializeEnergy = new UnityEvent<int>();
        [HideInInspector] public static UnityEvent<int> OnRefreshEnergy = new UnityEvent<int>();
        [HideInInspector] public static UnityEvent<int> OnRemoveEnergy = new UnityEvent<int>();
        [HideInInspector] public static UnityEvent<int> OnGainEnergy = new UnityEvent<int>();

        [SerializeField] private int startingEnergyCount;
        public int StartingEnergyCount => startingEnergyCount;
        public int currentEnergyCount { get; private set; }

        void Awake()
        {
            instance = this;
        }

        private void OnEnable()
        {
            ActionSystem.AttachPerformer<ConsumeEnergyGa>(ConsumeEnergyPerformer);
            ActionSystem.AttachPerformer<GainEnergyGa>(GainEnergyPerformer);
        }

        private void OnDisable()
        {
            ActionSystem.DetachPerformer<ConsumeEnergyGa>();
            ActionSystem.DetachPerformer<GainEnergyGa>();
        }
        
        private IEnumerator ConsumeEnergyPerformer(ConsumeEnergyGa consumeEnergyGa)
        {
            currentEnergyCount = Mathf.Max(0, currentEnergyCount - consumeEnergyGa.cost);

            OnRemoveEnergy.Invoke(consumeEnergyGa.cost);

            if (currentEnergyCount <= 0)
                OnOutOfEnergy.Invoke();
            
            yield break;
        }
        
        private IEnumerator GainEnergyPerformer(GainEnergyGa gainEnergyGa)
        {
            currentEnergyCount += gainEnergyGa.amount;
            
            OnGainEnergy?.Invoke(currentEnergyCount);
            
            yield break;
        }

        public bool CheckForEnergy(int energyRequiredForAction)
        {
            return energyRequiredForAction <= currentEnergyCount;
        }

        public void Initialize()
        {
            currentEnergyCount = startingEnergyCount;
            OnInitializeEnergy.Invoke(currentEnergyCount);
        }

        public void RefreshOnTurnStart()
        {
            currentEnergyCount = startingEnergyCount;
            OnRefreshEnergy.Invoke(currentEnergyCount);
        }
    }
}
