using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Spells
{
    public class DefaultSpellController : SpellController
    {
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            
            foreach (CardMovement target in targets)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                
                Debug.Log($"Target : {target.cardController.cardData.cardName} / {spellData.targetType}");

                if (spellData.inflictStatus != StatusType.None && spellData.statusStacksApplied > 0)
                {
                    ApplyStatusGa applyStatusGa = new ApplyStatusGa(spellData.inflictStatus, spellData.statusStacksApplied, cardController, target.cardController);
                    ActionSystem.instance.Perform(applyStatusGa);
                    yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                }

                if (spellData.damage > 0)
                {
                    DealDamageGA damageGa = new DealDamageGA(ComputeCurrentDamage(spellData.damage), cardController, target.cardController);
                    ActionSystem.instance.Perform(damageGa);
                    yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                }
            }
        }
    }
}
