using System.Collections;
using System.Collections.Generic;
using Cards.Scripts;
using Cards.Tween_Animations;
using Combat.Card_Container.CardSlot;
using Combat.Card_Container.Script;
using Run_Loop;
using UnityEngine;

namespace Map.Encounters.Fountain
{
    public class FountainEncounter : BasicEncounterInteraction
    {
        [SerializeField] private List<CardContainer> container;
        
        protected override IEnumerator DoStuffPostValidation()
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
            foreach (CardContainer stickyContainer in container)
            {
                for (int i = stickyContainer.Slots.Count - 1; i >= 0; i--)
                {
                    stickyContainer.Slots[i].CurrentCard.cardController.KillCard(false);
                }
            }

            ClearHand();
        }
    }
}
