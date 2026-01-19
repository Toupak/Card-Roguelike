using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.Spells.Data.PatientBear
{
    public class BearBuff : SpellController
    {
        private bool hasSpellBeenCasted;

        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            hasSpellBeenCasted = true;
            ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Armor, 1, cardController, cardController);
            ActionSystem.instance.Perform(applyStatusGa);
        }
        
        protected override void SubscribeReactions()
        {
            base.SubscribeReactions();
            ActionSystem.SubscribeReaction<StartTurnGa>(StartTurnReaction, ReactionTiming.POST);
        }

        protected override void UnsubscribeReactions()
        {
            base.UnsubscribeReactions();
            ActionSystem.UnsubscribeReaction<StartTurnGa>(StartTurnReaction, ReactionTiming.PRE);
        }

        private void StartTurnReaction(StartTurnGa startTurnGa)
        {
            if (hasSpellBeenCasted && startTurnGa.starting == CombatLoop.TurnType.Player)
            {
                hasSpellBeenCasted = false;
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.BonusDamage, 1, cardController, cardController);
                ActionSystem.instance.AddReaction(applyStatusGa);
            }
        }
    }
}
