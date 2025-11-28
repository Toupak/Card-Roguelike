using BoomLib.SFX_Player.Scripts;
using Cards.Scripts;
using UnityEngine;

public class CardSFX : MonoBehaviour
{
    [SerializeField] private AudioClip pickUp;
    [SerializeField] private AudioClip drop;

    private CardMovement cardMovement;

    private void OnEnable()
    {
        cardMovement = GetComponent<CardMovement>();

        cardMovement.OnStartDrag.AddListener(PlayPickUpSound);
        cardMovement.OnDrop.AddListener(PlayDropSound);
    }

    private void OnDisable()
    {
        cardMovement.OnStartDrag.RemoveListener(PlayPickUpSound);
        cardMovement.OnDrop.RemoveListener(PlayDropSound);
    }

    private void PlayPickUpSound()
    {
        SFXPlayer.instance.PlaySFX(pickUp);
    }

    private void PlayDropSound()
    {
        SFXPlayer.instance.PlaySFX(drop);
    }
}
