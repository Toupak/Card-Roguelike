using BoomLib.BoomTween;
using BoomLib.Tools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class EnergyDisplay : MonoBehaviour
{
    [SerializeField] private GameObject energyTokenPrefab;

    private List<Animator> tokens = new List<Animator>();
    private int count;

    public float fadeDuration;

    private bool isInitializing;
    private bool isResetting;
    private bool isUsing;
    
    void Start()
    {
        EnergyController.OnUpdateEnergy.AddListener(UpdateDisplay);
    }

    public void UpdateDisplay(int energy)
    {
        if (tokens.Count == 0)
        {
            StartInitializeCoroutine(energy);
            return;
        }

        else if (energy == EnergyController.instance.StartingEnergyCount)
            StartResetEnergyCoroutine(energy);

        else
            StartUseEnergyCoroutine(energy);
    }

    private void StartInitializeCoroutine(int currentEnergy) => StartCoroutine(InitializeEnergy(currentEnergy));
    private void StartUseEnergyCoroutine(int energy) => StartCoroutine(UseEnergy(energy));
    private void StartResetEnergyCoroutine(int energy) => StartCoroutine(ResetEnergy(energy));
    
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
}
