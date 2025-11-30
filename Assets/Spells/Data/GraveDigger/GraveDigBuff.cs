using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using CardSlot.Script;
using UnityEngine;

namespace Spells.Data.GraveDigger
{
    public class GraveDigBuff : NecroSpellController
    {
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            yield return ConsumeCorpses(corpseCost);

            foreach (CardMovement target in targets)
            {
                foreach (Slot slot in target.tokenContainer.Slots)
                {
                    yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                    ApplyStatusGa applyStatusGa = new ApplyStatusGa(spellData.inflictStatus, spellData.statusStacksApplied, cardController, slot.CurrentCard.cardController);
                    ActionSystem.instance.Perform(applyStatusGa);
                }
            }
        }
    }
}
