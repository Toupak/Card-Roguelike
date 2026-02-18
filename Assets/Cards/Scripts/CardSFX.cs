using System.Collections.Generic;
using BoomLib.SFX_Player.Scripts;
using Combat.Card_Container.Script;
using UnityEngine;

namespace Cards.Scripts
{
    public class CardSFX : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> pickUp;
        [SerializeField] private List<AudioClip> dropHand;
        [SerializeField] private List<AudioClip> dropBoard;
        
        [Space]
        [SerializeField] private List<AudioClip> inspect;
        [SerializeField] private List<AudioClip> stopInspect;
        
        [Space]
        [SerializeField] private List<AudioClip> spawn;
        [SerializeField] private List<AudioClip> takeDamage;
        [SerializeField] private List<AudioClip> death;

        private CardController cardController;
        private CardMovement cardMovement;
        
        public void Setup(CardController controller, CardMovement movement, CardData cardData, CardHealth cardHealth)
        {
            cardController = controller;
            cardMovement = movement;
            
            cardMovement.OnStartDrag.AddListener(() => PlaySFXOrDefault(cardData.pickupAudioClip, pickUp));
            cardMovement.OnDrop.AddListener(() =>
            {
                if (cardMovement.ContainerType == CardContainer.ContainerType.Hand)
                    PlaySFXOrDefault(cardData.dropAudioClip, dropHand);
                else    
                    PlaySFXOrDefault(cardData.dropAudioClip, dropBoard);
            });
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

        private void PlaySFXOrDefault(List<AudioClip> clip, List<AudioClip> defaultClip)
        {
            if (clip != null && clip.Count > 0)
                PlaySFX(clip);
            else
                PlaySFX(defaultClip);
        }

        private void PlaySFX(List<AudioClip> clip)
        {
            if (clip != null && clip.Count > 0)
                SFXPlayer.instance.PlayRandomSFX(clip);
        }
    }
}
