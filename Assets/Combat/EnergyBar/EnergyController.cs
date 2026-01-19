using System.Collections;
using ActionReaction;
using ActionReaction.Game_Actions;
using UnityEngine;
using UnityEngine.Events;

namespace Combat.EnergyBar
{
    public class EnergyController : MonoBehaviour
    {
        public static EnergyController instance;

        [HideInInspector] public UnityEvent OnUpdateEnergy = new UnityEvent();

        private const int baseEnergyCount = 3;

        public int currentMaxEnergy => baseEnergyCount + bonusEnergy;
        public int currentEnergy { get; private set; }
        public int bonusEnergy { get; private set; }
        
        private void Awake()
        {
            instance = this;
        }
        
        public void Initialize()
        {
            RefillEnergy();
        }

        public void RefillEnergy()
        {
            bonusEnergy = 0;
            currentEnergy = baseEnergyCount;

            OnUpdateEnergy.Invoke();
        }
        
        public bool CheckForEnergy(int energyRequiredForAction)
        {
            return energyRequiredForAction <= currentEnergy;
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
            currentEnergy = Mathf.Max(0, currentEnergy - consumeEnergyGa.cost);
            
            OnUpdateEnergy.Invoke();

            yield break;
        }
        
        private IEnumerator GainEnergyPerformer(GainEnergyGa gainEnergyGa)
        {
            bonusEnergy += gainEnergyGa.amount;
            currentEnergy += gainEnergyGa.amount;
            
            OnUpdateEnergy.Invoke();
            
            yield break;
        }
    }
}
