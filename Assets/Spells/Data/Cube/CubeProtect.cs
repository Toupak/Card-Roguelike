using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Spells.Data.Cube
{
    public class CubeProtect : SpellController
    {
        private List<CardController> protectionTargets = new List<CardController>();
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            protectionTargets = new List<CardController>();

            foreach (CardMovement target in targets)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Protected, 1, cardController, target.cardController);
                ActionSystem.instance.Perform(applyStatusGa);
                protectionTargets.Add(target.cardController);
            }
        }
        
        protected override void SubscribeReactions()
        {
            base.SubscribeReactions();
            ActionSystem.SubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.PRE, 100);
            ActionSystem.SubscribeReaction<ApplyStatusGa>(ApplyStatusReaction, ReactionTiming.PRE, 100);
        }

        protected override void UnsubscribeReactions()
        {
            base.UnsubscribeReactions();
            ActionSystem.UnsubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.PRE);
            ActionSystem.UnsubscribeReaction<ApplyStatusGa>(ApplyStatusReaction, ReactionTiming.PRE);
        }
        
        protected override void EndTurnRefreshCooldownReaction(StartTurnGa startTurnGa)
        {
            base.EndTurnRefreshCooldownReaction(startTurnGa);
            if (startTurnGa.starting == CombatLoop.CombatLoop.TurnType.Player)
                protectionTargets = new List<CardController>();
        }

        private void DealDamageReaction(DealDamageGA dealDamageGa)
        {
            if (dealDamageGa.target != null && IsCardProtected(dealDamageGa.target))
                dealDamageGa.SwitchTarget(cardController);
        }
        
        private void ApplyStatusReaction(ApplyStatusGa applyStatusGa)
        {
            if (applyStatusGa.type != StatusType.Protected && applyStatusGa.target != null && IsCardProtected(applyStatusGa.target))
                applyStatusGa.SwitchTarget(cardController);
        }

        private bool IsCardProtected(CardController card)
        {
            return protectionTargets != null && protectionTargets.Count > 0 && protectionTargets.Contains(card);
        }
    }
}
