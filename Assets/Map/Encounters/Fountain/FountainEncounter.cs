using System.Collections;
using System.Collections.Generic;
using Cards.Scripts;
using Cards.Tween_Animations;
using Combat.Card_Container.Script;
using Map.Rooms;
using Run_Loop;
using UnityEngine;

namespace Map.Encounters.Fountain
{
    public class FountainEncounter : BasicEncounterInteraction
    {
        [SerializeField] private List<CardContainer> container;
        
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
            yield return HealCards();
            currentCharacterInteract.ExitInteractRange(this);
            RoomBuilder.instance.MarkCurrentRoomAsCleared();
            isUsed = true;
            animator.Play("Used");
        }

        private IEnumerator HealCards()
        {
            foreach (CardContainer stickyContainer in container)
            {
                if (stickyContainer.slotCount > 0)
                {
                    CardController card = stickyContainer.Slots[0].CurrentCard.cardController;
                    
                    yield return CardTween.NewPlaySelfAction(card);

                    card.cardHealth.Heal(10);
                    PlayerDeck.instance.UpdateCardHealthPoints(card.deckCard, card.cardHealth.currentHealth);
                }
            }
        }
        
        protected override void DoStuffPostClosing()
        {
            ClearContainers();
            ClearHand();
        }

        private void ClearContainers()
        {
            foreach (CardContainer stickyContainer in container)
            {
                for (int i = stickyContainer.Slots.Count - 1; i >= 0; i--)
                {
                    stickyContainer.Slots[i].CurrentCard.cardController.KillCard(false);
                }
            }
        }
    }
}
