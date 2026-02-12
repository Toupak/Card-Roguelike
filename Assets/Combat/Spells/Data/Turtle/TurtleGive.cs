using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.Spells.Data.Turtle
{
    public class TurtleGive : SpellController
    {
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            
            if (targets.Count < 2)
                yield break;

            int firstTargetStacks = targets[0].cardController.cardStatus.GetCurrentStackCount(spellData.inflictStatus);
            int secondTargetStacks = targets[1].cardController.cardStatus.GetCurrentStackCount(spellData.inflictStatus);
            
            if (firstTargetStacks > 0)
            {
                ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(spellData.inflictStatus, firstTargetStacks, cardController, targets[0].cardController);
                ActionSystem.instance.Perform(consumeStacksGa);
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            }
            
            if (secondTargetStacks > 0)
            {
                ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(spellData.inflictStatus, secondTargetStacks, cardController, targets[1].cardController);
                ActionSystem.instance.Perform(consumeStacksGa);
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            }
            
            if (firstTargetStacks > 0)
            {
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(spellData.inflictStatus, firstTargetStacks, cardController, targets[1].cardController);
                ActionSystem.instance.Perform(applyStatusGa);
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            }
            
            if (secondTargetStacks > 0)
            {
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(spellData.inflictStatus, secondTargetStacks, cardController, targets[0].cardController);
                ActionSystem.instance.Perform(applyStatusGa);
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            }
        }
    }
}
