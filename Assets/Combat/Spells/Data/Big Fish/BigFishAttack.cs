using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.Spells.Data.Big_Fish
{
    public class BigFishAttack : SpellController
    {
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            int damage = ComputeCurrentDamage(spellData.damage);
            foreach (CardMovement target in targets)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                
                DealDamageGA dealDamageGa = new DealDamageGA(damage, cardController, target.cardController);
                ActionSystem.instance.Perform(dealDamageGa);
            }

            if (damage > 0)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Armor, damage, cardController, cardController);
                ActionSystem.instance.Perform(applyStatusGa);
            }
        }
    }
}
