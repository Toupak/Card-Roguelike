using System.Collections;
using Cards.Scripts;
using Cards.Tween_Animations;
using Combat.Card_Container.Script;
using Map.Rooms;
using Run_Loop;
using UnityEngine;

namespace Map.Encounters.Altar
{
    public class AltarEncounter : BasicEncounterInteraction
    {
        [SerializeField] private CardContainer deathContainer;
        [SerializeField] private CardContainer healContainer;
        
        private Animator animator;
        
        private bool isUsed;
        
        public override bool CanInteract()
        {
            return !isUsed;
        }
        
        protected override void Setup()
        {
            base.Setup();
            animator = GetComponent<Animator>();
            
            isUsed = RoomBuilder.instance.HasRoomBeenCleared();
            if (isUsed)
                animator.Play("Used");
        }
        
        protected override IEnumerator DoStuffPostValidation()
        {
            yield return SacrificeCard();
            currentCharacterInteract.ExitInteractRange(this);
            RoomBuilder.instance.MarkCurrentRoomAsCleared();
            animator.Play("Used");
        }

        private IEnumerator SacrificeCard()
        {
            bool hasHealedACard = false;
            
            if (deathContainer.slotCount > 0 && healContainer.slotCount > 0)
            {
                CardController sacrificedCard = deathContainer.Slots[0].CurrentCard.cardController;
                CardController healedCard = healContainer.Slots[0].CurrentCard.cardController;

                int healValue = sacrificedCard.cardHealth.currentHealth;
                
                yield return CardTween.NewPlaySelfAction(sacrificedCard);
                sacrificedCard.KillCard();
                
                yield return CardTween.NewPlaySelfAction(healedCard);
                healedCard.cardHealth.Heal(healValue);
                PlayerDeck.instance.UpdateCardHealthPoints(healedCard.deckCard, healedCard.cardHealth.currentHealth);

                hasHealedACard =  true;
            }
            
            isUsed = hasHealedACard;
        }
        
        protected override void DoStuffPostClosing()
        {
            ClearContainers();
            ClearHand();
        }

        private void ClearContainers()
        {
            if (deathContainer.slotCount > 0)
                deathContainer.Slots[0].CurrentCard.cardController.KillCard(false);
            if (healContainer.slotCount > 0)
                healContainer.Slots[0].CurrentCard.cardController.KillCard(false);
        }
    }
}
