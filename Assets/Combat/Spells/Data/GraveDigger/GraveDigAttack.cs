using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.Spells.Data.GraveDigger
{
    public class GraveDigAttack : SpellController
    {
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            foreach (CardMovement target in targets)
            {
                DealDamageGA dealDamageGa = new DealDamageGA(ComputeCurrentDamage(spellData.damage), cardController, target.cardController);
                ActionSystem.instance.Perform(dealDamageGa);
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                if (!dealDamageGa.isDamageNegated && dealDamageGa.amount > 0)
                {
                    ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Corpse, spellData.statusStacksApplied, cardController, cardController);
                    ActionSystem.instance.Perform(applyStatusGa);
                    yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                }
            }
        }
    }
}
