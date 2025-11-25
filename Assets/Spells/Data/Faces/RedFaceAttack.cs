using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using CardSlot.Script;
using UnityEngine;

namespace Spells.Data.Faces
{
    public class RedFaceAttack : SpellController
    {
        public override bool CanCastSpell()
        {
            return base.CanCastSpell() && cardController.cardMovement.tokenContainer.slotCount > 0;
        }
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            foreach (Slot slot in cardController.cardMovement.tokenContainer.Slots)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                ApplyStatusGa applyStatusGa = new ApplyStatusGa(spellData.inflictStatus, spellData.statusStacksApplied, cardController, slot.CurrentCard.cardController);
                ActionSystem.instance.Perform(applyStatusGa);
            }
        }
    }
}
