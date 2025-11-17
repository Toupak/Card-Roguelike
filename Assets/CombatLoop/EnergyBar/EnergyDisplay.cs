using System.Collections;
using System.Collections.Generic;
using Spells.Data.GnomeCarrier;
using UnityEngine;

namespace CombatLoop.EnergyBar
{
    public class EnergyDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject energyTokenPrefab;

        private List<Animator> tokens = new List<Animator>();
        private int count;
        
        private bool isInitializing;
        private bool isResetting;
        private bool isUsing;
    
        void Start()
        {
            EnergyController.OnInitializeEnergy.AddListener(StartInitializeCoroutine);
            EnergyController.OnRefreshEnergy.AddListener(StartResetEnergyCoroutine);
            EnergyController.OnRemoveEnergy.AddListener(StartUseEnergyCoroutine);
            EnergyController.OnGainEnergy.AddListener(StartGainEnergyCoroutine);
        }

        private void StartInitializeCoroutine(int currentEnergy) => StartCoroutine(InitializeEnergy(currentEnergy));
        private void StartResetEnergyCoroutine(int energy) => StartCoroutine(ResetEnergy(energy));
        private void StartUseEnergyCoroutine(int energy) => StartCoroutine(UseEnergy(energy));
        private void StartGainEnergyCoroutine(int energy) => StartCoroutine(Gain(energy));

        private IEnumerator InitializeEnergy(int startingEnergy)
        {
            if (isInitializing)
                yield break;

            isInitializing = true;

            for (int i = 0; i < startingEnergy; i++)
            {
                GameObject energyToken = Instantiate(energyTokenPrefab, transform);
                Animator tokenAnimator = energyToken.GetComponent<Animator>();
                tokens.Add(tokenAnimator);
                
                count += 1;

                if (tokenAnimator != null)
                {
                    tokenAnimator.Play("Spawning");
                    yield return new WaitUntil(() => tokenAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
                }
            }

            isInitializing = false;
        }

        private IEnumerator ResetEnergy(int energy)
        {
            if (isResetting)
                yield break;
            
            if (energy == count)
                yield break;

            for (int i = tokens.Count - 1; i >= energy; i--)
            {
                Destroy(tokens[i].gameObject);
            }

            isResetting = true;

            for (int i = count; i < energy; i++)
            {
                if (count <= energy)
                {
                    tokens[count].Play("Spawning");
                    count += 1;
                }
            }

            isResetting = false;
        }

        private IEnumerator UseEnergy(int energyUsed)
        {
            if (isUsing)
                yield break;

            isUsing = true;

            for (int i = 0; i < energyUsed; i++)
            {
                if (count >= 0)
                {
                    tokens[count - 1].Play("Using");
                    yield return new WaitUntil(() => tokens[count - 1].GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
                    count -= 1;
                }
            }

            isUsing = false;
        }
        
        private IEnumerator Gain(int energyGained)
        {
            if (isUsing)
                yield break;
            isUsing = true;

            for (int i = count; i < energyGained; i++)
            {
                if (count <= energyGained)
                {
                    if (count < tokens.Count)
                        tokens[count].Play("Spawning");
                    else
                        SpawnToken();
                    count += 1;
                }
            }

            isUsing = false;
        }

        private void SpawnToken()
        {
            GameObject energyToken = Instantiate(energyTokenPrefab, transform);
            Animator tokenAnimator = energyToken.GetComponent<Animator>();
            tokens.Add(tokenAnimator);
            tokenAnimator.Play("Spawning");
        }
    }
}
