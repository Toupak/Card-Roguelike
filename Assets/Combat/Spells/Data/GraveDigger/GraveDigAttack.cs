using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using BoomLib.Tools;
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

            DealDamageGA dealDamageGa = new DealDamageGA(ComputeCurrentDamage(spellData.damage), cardController, targets.GetControllers());
            ActionSystem.instance.Perform(dealDamageGa);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            foreach (DealDamageGA.DamagePackage package in dealDamageGa.packages)
            {
                if (!package.isDamageNegated && package.amount > 0)
                {
                    ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Corpse, package.amount, cardController, cardController);
                    ActionSystem.instance.Perform(applyStatusGa);
                    yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                }
            }
        }
    }
}
