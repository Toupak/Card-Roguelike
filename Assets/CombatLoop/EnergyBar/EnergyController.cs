using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class EnergyController : MonoBehaviour
{
    public static EnergyController instance;

    [HideInInspector] public static UnityEvent OnOutOfEnergy = new UnityEvent();
    [HideInInspector] public static UnityEvent<int> OnUpdateEnergy = new UnityEvent<int>();

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
        OnUpdateEnergy.Invoke(currentEnergyCount);
    }

    public void RefreshOnTurnStart()
    {
        currentEnergyCount = startingEnergyCount;
        OnUpdateEnergy.Invoke(currentEnergyCount);
    }

    public void RemoveEnergy(int energyRequiredForAction)
    {
        currentEnergyCount -= energyRequiredForAction;

        OnUpdateEnergy.Invoke(energyRequiredForAction);

        if (currentEnergyCount <= 0)
            OnOutOfEnergy.Invoke();
    }
}
