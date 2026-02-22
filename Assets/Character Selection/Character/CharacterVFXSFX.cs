using BoomLib.SFX_Player.Scripts;
using Character_Selection.Character;
using System;
using UnityEngine;

public class CharacterVFXSFX : MonoBehaviour
{
    [SerializeField] private AudioClip SFX_rightClick;
    [SerializeField] private GameObject VFX_hasPressedRightClick;

    [SerializeField] private GameObject VFX_isPressingRightClick;
    [SerializeField] private float antiSpamCD_RightClickVFX;
    [SerializeField] private float delayBeforeSpawnCD_RightClickVFX;
    private float lastRightClickVFXTimeStamp;
    private float lastRightClickTimeStamp;
    private Vector2 hasClickedPosition;

    private void OnEnable()
    {
        CharacterMovement.OnRightClickSetTarget.AddListener(DoRightClickStuff);
    }

    private void OnDisable()
    {
        CharacterMovement.OnRightClickSetTarget.RemoveListener(DoRightClickStuff);
    }

    private void DoRightClickStuff(Vector2 VFXPosition, bool hasClicked)
    {
        if (hasClicked)
        {
            SFXPlayer.instance.PlaySFX(SFX_rightClick);
            Instantiate(VFX_hasPressedRightClick, VFXPosition, Quaternion.identity);
            hasClickedPosition = VFXPosition;

            lastRightClickTimeStamp = Time.time;
        }
        
        if (CanInstantiateRightClickVFX(hasClicked, hasClickedPosition, VFXPosition))
        {
            lastRightClickVFXTimeStamp = Time.time;
            Instantiate(VFX_isPressingRightClick, VFXPosition, Quaternion.identity);
        }
    }
    private bool CanInstantiateRightClickVFX(bool hasClicked, Vector2 hasClickedPosition, Vector2 VFXPosition)
    {
        bool VFXAntispam = Time.time - lastRightClickVFXTimeStamp > antiSpamCD_RightClickVFX;
        bool DelayBeforeSpawn = Time.time - lastRightClickTimeStamp > delayBeforeSpawnCD_RightClickVFX;

        return !hasClicked && VFXAntispam && DelayBeforeSpawn;
    }
}