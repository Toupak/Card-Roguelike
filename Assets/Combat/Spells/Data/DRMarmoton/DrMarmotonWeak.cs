using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.Spells.Data.DRMarmoton
{
    public class DrMarmotonWeak : SpellController
    {
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            foreach (CardMovement target in targets)
            {
                int weakCount = target.cardController.cardStatus.GetCurrentStackCount(StatusType.Weak);

                if (weakCount < 1)
                    continue;

                ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.Weak, weakCount, cardController, target.cardController);
                ActionSystem.instance.Perform(consumeStacksGa);
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.BonusDamage, weakCount, cardController, target.cardController);
                ActionSystem.instance.Perform(applyStatusGa);
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            }
        }
    }
}
