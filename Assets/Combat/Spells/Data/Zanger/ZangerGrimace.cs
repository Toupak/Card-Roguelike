using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using BoomLib.Tools;
using Cards.Scripts;
using Combat.Spells.Targeting;
using UnityEngine;

namespace Combat.Spells.Data.Zanger
{
    public class ZangerGrimace : SpellController
    {
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            bool isObedient = cardController.cardStatus.IsStatusApplied(StatusType.Obedience);
            bool attackAlly = !isObedient && Tools.RandomBool();

            CardController target = targets[0].cardController;
            
            if (attackAlly)
            {
                targets = TargetingSystem.instance.RetrieveBoard(TargetType.Ally);
                int randomTarget = Random.Range(0, targets.Count);
                target = targets[randomTarget].cardController;
            }

            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            ApplyStatusGa applyStatusGa = new ApplyStatusGa(spellData.inflictStatus, spellData.statusStacksApplied, cardController, target);
            ActionSystem.instance.Perform(applyStatusGa);

            if (isObedient)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                ConsumeStacksGa consume = new ConsumeStacksGa(StatusType.Obedience, 1, cardController, cardController);
                ActionSystem.instance.Perform(consume);
            }
        }
    }
}
