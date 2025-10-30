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

    private List<Image> tokens = new List<Image>();
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
            Image tokenImage = energyToken.GetComponent<Image>();

            tokens.Add(tokenImage);
            count += 1;

            if (tokenImage != null)
            {
                Debug.Log("Toup: ca fade 1");
                yield return Fader.Fade(tokenImage, fadeDuration, true);
                Debug.Log("Toup: ca fade 2");

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
                yield return Fader.Fade(tokens[count], fadeDuration, true);
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
                yield return Fader.Fade(tokens[count - 1], fadeDuration, false);
                count -= 1;
            }
        }

        isUsing = false;
    }
}
