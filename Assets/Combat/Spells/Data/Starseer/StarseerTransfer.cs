using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.Spells.Data.Starseer
{
    public class StarseerTransfer : SpellController
    {
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            CardController target = targets[0].cardController;
            List<StatusHolder> stacks = cardController.cardStatus.CurrentStacks;

            foreach (StatusHolder status in stacks.ToList())
            {
                int stackCount = status.stackCount;
                if (stackCount < 1)
                    continue;
                
                ConsumeStacksGa consume = new ConsumeStacksGa(status.statusType, stackCount, null, cardController);
                ActionSystem.instance.Perform(consume);

                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                ApplyStatusGa apply = new ApplyStatusGa(status.statusType, stackCount, cardController, target);
                ActionSystem.instance.Perform(apply);
                
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            }
        }
    }
}
