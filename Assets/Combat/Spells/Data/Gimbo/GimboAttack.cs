using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.Spells.Data.Gimbo
{
    public class GimboAttack : SpellController
    {
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            int targetCount = ComputeCurrentTargetCount(spellData.targetCount);

            for (int i = 0; i < targetCount && targets.Count > 0; i++)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                CardController target = PickRandomTarget(targets);
                targets.Remove(target.cardMovement);
                
                int stunStacks = target.cardStatus.GetCurrentStackCount(StatusType.Stun);

                if (stunStacks > 0)
                {
                    ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.Stun, stunStacks, cardController, target);
                    ActionSystem.instance.Perform(consumeStacksGa);
                    
                    yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                }
                
                DealDamageGA dealDamageGa = new DealDamageGA(ComputeCurrentDamage(spellData.damage) + stunStacks, cardController, target);
                ActionSystem.instance.Perform(dealDamageGa);
            }
        }
    }
}
