using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.Spells.Data.DRMarmoton
{
    public class DrMarmotonStun : SpellController
    {
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            foreach (CardMovement target in targets)
            {
                int stunCount = target.cardController.cardStatus.currentStacks.TryGetValue(StatusType.Stun, out var stack) ? stack : 0;

                if (stunCount < 1)
                    continue;

                ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.Stun, stunCount, cardController, target.cardController);
                ActionSystem.instance.Perform(consumeStacksGa);
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Weak, stunCount, cardController, target.cardController);
                ActionSystem.instance.Perform(applyStatusGa);
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            }
        }
    }
}
