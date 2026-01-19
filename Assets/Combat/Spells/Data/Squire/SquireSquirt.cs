using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Spells.Data.Squire
{
    public class SquireSquirt : SpellController
    {
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            
            foreach (CardMovement target in targets)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                
                Debug.Log($"Target : {target.cardController.cardData.cardName} / {spellData.targetType}");
                
                int damageMultiplier = target.cardController.cardStatus.IsStatusApplied(StatusType.Stun) ? 2 : 1;
                DealDamageGA damageGa = new DealDamageGA(ComputeCurrentDamage(spellData.damage) * damageMultiplier, cardController, target.cardController);
                ActionSystem.instance.Perform(damageGa);
            }
        }
    }
}
