using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.Spells.Data.Frien
{
    public class FrienAttack : SpellController
    {
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            int damage = ComputeCurrentDamage(spellData.damage);

            foreach (CardMovement target in targets)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                int bonusDamage = target.cardController.cardStatus.IsStatusApplied(StatusType.Terror) ? 2 : 0;
                
                DealDamageGA dealDamageGa = new DealDamageGA(damage + bonusDamage, cardController, target.cardController);
                ActionSystem.instance.Perform(dealDamageGa);

                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                ApplyStatusGa applyStatusGa = new ApplyStatusGa(spellData.inflictStatus, spellData.statusStacksApplied, cardController, target.cardController);
                ActionSystem.instance.Perform(applyStatusGa);
            }
        }
    }
}
