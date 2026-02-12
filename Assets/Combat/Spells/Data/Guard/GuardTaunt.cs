using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.Spells.Data.Guard
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
            ActionSystem.SubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.POST, 10);
        }

        protected override void UnsubscribeReactions()
        {
            base.UnsubscribeReactions();
            ActionSystem.UnsubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.POST);
        }

        private void DealDamageReaction(DealDamageGA dealDamageGa)
        {
            if (dealDamageGa.IsCardTargeted(cardController) && cardController.cardStatus.IsStatusApplied(StatusType.Taunt) && dealDamageGa.attacker != null && dealDamageGa.attacker.cardStatus.IsStatusApplied(StatusType.Vengeance))
            {
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Taunt, 1, cardController, cardController);
                ActionSystem.instance.AddReaction(applyStatusGa);
            }
        }
    }
}
