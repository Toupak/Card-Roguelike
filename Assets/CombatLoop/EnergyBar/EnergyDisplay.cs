using System.Collections.Generic;
using UnityEngine;

namespace CombatLoop.EnergyBar
{
    public class EnergyDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject energyTokenPrefab;

        private Dictionary<Animator, bool> tokens = new Dictionary<Animator, bool>();
        
        private EnergyController energyController;
        
        private void Start()
        {
            energyController = EnergyController.instance;
            energyController.OnUpdateEnergy.AddListener(UpdateEnergyDisplay);
        }

        private void UpdateEnergyDisplay()
        {
            int currentEnergy = energyController.currentEnergy;
            int maxEnergy = energyController.currentMaxEnergy;

            if (tokens.Count < maxEnergy)
                SpawnTokens(maxEnergy - tokens.Count);
            else if (tokens.Count > maxEnergy)
                DestroyTokens(tokens.Count - maxEnergy);

            UpdateTokensDisplay(currentEnergy);
        }

        private void UpdateTokensDisplay(int currentEnergy)
        {
            List<Animator> keys = new List<Animator>(tokens.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                PlayTokenAnimation(keys[i], i < currentEnergy);
            }
        }

        private void PlayTokenAnimation(Animator animator, bool state)
        {
            if (tokens[animator] == state)
                return;

            tokens[animator] = state;
            animator.Play(state ? "Spawning" : "Using");
        }

        private void SpawnTokens(int tokenCount)
        {
            for (int i = 0; i < tokenCount; i++)
            {
                GameObject energyToken = Instantiate(energyTokenPrefab, transform);
                Animator tokenAnimator = energyToken.GetComponent<Animator>();
                tokens.Add(tokenAnimator, false);
            }
        }
        
        private void DestroyTokens(int tokensCount)
        {
            List<Animator> keys = new List<Animator>(tokens.Keys);
            int lastIndex = keys.Count - 1;
            for (int i = 0; i < tokensCount; i++)
            {
                int index = lastIndex - i;
                Destroy(keys[index].gameObject);
                tokens.Remove(keys[index]);
            }
        }
    }
}
