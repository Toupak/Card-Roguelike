using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Spells.Data.Guard
{
    public class GuardTaunt : SpellController
    {
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            
            ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Taunt, spellData.statusStacksApplied, cardController, cardController);
            ActionSystem.instance.Perform(applyStatusGa);
        }
        
        protected override void SubscribeReactions()
        {
            base.SubscribeReactions();
            ActionSystem.SubscribeReaction<ConsumeStacksGa>(ConsumeStacksReaction, ReactionTiming.POST);
        }

        protected override void UnsubscribeReactions()
        {
            base.UnsubscribeReactions();
            ActionSystem.UnsubscribeReaction<ConsumeStacksGa>(ConsumeStacksReaction, ReactionTiming.POST);
        }

        private void ConsumeStacksReaction(ConsumeStacksGa consumeStacksGa)
        {
            if (consumeStacksGa.target == cardController && consumeStacksGa.type == StatusType.Taunt && consumeStacksGa.attacker.cardStatus.IsStatusApplied(StatusType.Vengeance))
            {
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Taunt, 1, cardController, cardController);
                ActionSystem.instance.AddReaction(applyStatusGa);
            }
        }
    }
}
