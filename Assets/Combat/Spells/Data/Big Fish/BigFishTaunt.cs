using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.Spells.Data.Big_Fish
{
    public class BigFishTaunt : SpellController
    {
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            int stacks = 0;
            foreach (CardMovement target in targets)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                int targetStacks = target.cardController.cardStatus.GetCurrentStackCount(StatusType.Taunt);
                ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.Taunt, targetStacks, cardController, target.cardController);
                ActionSystem.instance.Perform(consumeStacksGa);
                
                stacks += targetStacks;
            }
            
            if (stacks < 1)
                yield break;
            
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Taunt, stacks, cardController, cardController);
            ActionSystem.instance.Perform(applyStatusGa);
            
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            ApplyStatusGa applyBonusDamage = new ApplyStatusGa(StatusType.BonusDamage, stacks, cardController, cardController);
            ActionSystem.instance.Perform(applyBonusDamage);
        }
    }
}
