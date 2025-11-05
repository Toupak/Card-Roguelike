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

        [SerializeField] private int startingEnergyCount;
        public int StartingEnergyCount => startingEnergyCount;
        public int currentEnergyCount { get; private set; }

        void Awake()
        {
            instance = this;
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

        public void RemoveEnergy(int energyRequiredForAction)
        {
            currentEnergyCount -= energyRequiredForAction;

            OnRemoveEnergy.Invoke(energyRequiredForAction);

            if (currentEnergyCount <= 0)
                OnOutOfEnergy.Invoke();
        }
    }
}
