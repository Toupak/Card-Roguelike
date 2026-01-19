using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Card_Container.CardSlot;
using UnityEngine;

namespace Combat.Spells.Data.Snuk
{
    public class SnukBuff : SpellController
    {
        [SerializeField] private CardData eggData;
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            List<CardController> eggs = new List<CardController>();
            
            foreach (Slot slot in cardController.cardMovement.tokenContainer.Slots)
            {
                if (slot.CurrentCard.cardController.cardData.name == eggData.name)
                    eggs.Add(slot.CurrentCard.cardController);
            }

            foreach (CardController egg in eggs)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                DeathGA deathGa = new DeathGA(cardController, egg);
                ActionSystem.instance.Perform(deathGa);
                    
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.BonusDamage, 1, cardController, cardController);
                ActionSystem.instance.Perform(applyStatusGa);
            }
        }
    }
}
