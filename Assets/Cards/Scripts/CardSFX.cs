using BoomLib.SFX_Player.Scripts;
using Combat.Card_Container.Script;
using UnityEngine;

namespace Cards.Scripts
{
    public class CardSFX : MonoBehaviour
    {
        [SerializeField] private AudioClip pickUp;
        [SerializeField] private AudioClip drop;
        
        [Space]
        [SerializeField] private AudioClip inspect;
        [SerializeField] private AudioClip stopInspect;
        
        [Space]
        [SerializeField] private AudioClip spawn;
        [SerializeField] private AudioClip takeDamage;
        [SerializeField] private AudioClip death;

        private CardController cardController;
        private CardMovement cardMovement;
        
        public void Setup(CardController controller, CardMovement movement, CardData cardData, CardHealth cardHealth)
        {
            cardController = controller;
            cardMovement = movement;
            
            cardMovement.OnStartDrag.AddListener(() => PlaySFXOrDefault(cardData.pickupAudioClip, pickUp));
            cardMovement.OnDrop.AddListener(() => PlaySFXOrDefault(cardData.dropAudioClip, drop));
            cardMovement.OnSelected.AddListener(() => PlaySFXOrDefault(cardData.inspectAudioClip, inspect));
            cardMovement.OnDeselected.AddListener(() => PlaySFXOrDefault(cardData.stopInspectAudioClip, stopInspect));
            cardMovement.OnSetNewSlot.AddListener(() =>
            {
                if (cardMovement.ContainerType == CardContainer.ContainerType.Board)
                    PlaySFXOrDefault(cardData.spawnAudioClip, spawn);
            });
            
            cardHealth.OnTakeDamage.AddListener(() => PlaySFXOrDefault(cardData.takeDamageAudioClip, takeDamage));
            cardController.OnKillCard.AddListener(() => PlaySFXOrDefault(cardData.deathAudioClip, death));
        }

        private void PlaySFXOrDefault(AudioClip clip, AudioClip defaultClip)
        {
            if (clip != null)
                PlaySFX(clip);
            else
                PlaySFX(defaultClip);
        }

        private void PlaySFX(AudioClip clip)
        {
            if (clip != null)
                SFXPlayer.instance.PlaySFX(clip);
        }
    }
}
