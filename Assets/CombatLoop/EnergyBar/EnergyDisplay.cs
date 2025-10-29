using UnityEngine;

public class EnergyDisplay : MonoBehaviour
{
    [SerializeField] private GameObject energyTokenPrefab;
    
    void Start()
    {
        EnergyController.OnUpdateEnergy.AddListener(UpdateDisplay);
    }

    public void UpdateDisplay(int currentEnergy)
    {
        if (currentEnergy == EnergyController.instance.StartingEnergyCount)
            InitializeEnergy(currentEnergy);
        else
            DeleteEnergy(currentEnergy);
    }

    private void InitializeEnergy(int currentEnergy)
    {
        for (int i = 0; i < currentEnergy; i++)
        {
            GameObject energyToken = Instantiate(energyTokenPrefab);
            energyToken.transform.SetParent(transform);
        }
    }

    private void DeleteEnergy(int currentEnergy)
    {
        for (int i = 0; i <= currentEnergy; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
