using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Spells.Data.Turtle
{
    public class TurtleReceive : SpellController
    {
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            foreach (CardMovement target in targets)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                
                int targetStacks = target.cardController.cardStatus.IsStatusApplied(spellData.inflictStatus)
                    ? target.cardController.cardStatus.currentStacks[spellData.inflictStatus]
                    : 0;
                
                ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(spellData.inflictStatus, targetStacks, cardController, target.cardController);
                ActionSystem.instance.Perform(consumeStacksGa);
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(spellData.inflictStatus, targetStacks, cardController, cardController);
                ActionSystem.instance.Perform(applyStatusGa);
            }
        }
    }
}
