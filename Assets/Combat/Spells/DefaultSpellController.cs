using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using BoomLib.Tools;
using Cards.Scripts;
using UnityEngine;

namespace Combat.Spells
{
    public class DefaultSpellController : SpellController
    {
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            
            if (spellData.inflictStatus != StatusType.None && spellData.statusStacksApplied > 0)
            {
                foreach (CardMovement target in targets)
                {
                    ApplyStatusGa applyStatusGa = new ApplyStatusGa(spellData.inflictStatus, spellData.statusStacksApplied, cardController, target.cardController);
                    ActionSystem.instance.Perform(applyStatusGa);
                    yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                }
            }

            if (spellData.damage > 0)
            {
                if (targets.Count > 1)
                {
                    DealDamageGA damageGa = new DealDamageGA(ComputeCurrentDamage(spellData.damage), cardController, targets.GetControllers());
                    ActionSystem.instance.Perform(damageGa);
                }
                else if (targets.Count == 1)
                {
                    DealDamageGA damageGa = new DealDamageGA(ComputeCurrentDamage(spellData.damage), cardController, targets[0].cardController);
                    ActionSystem.instance.Perform(damageGa);
                }
            }
            
        }
    }
}
