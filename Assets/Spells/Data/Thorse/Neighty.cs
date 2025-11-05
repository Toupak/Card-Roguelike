using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Spells.Targeting;

namespace Spells.Data.Thorse
{
    public class Neighty : SpellController
    {
        protected override void SubscribeReactions()
        {
            base.SubscribeReactions();
            ActionSystem.SubscribeReaction<ApplyStatusGa>(ImmuneToStunReaction, ReactionTiming.PRE);
            ActionSystem.SubscribeReaction<DeathGA>(KillEnemyReaction, ReactionTiming.POST);
        }

        protected override void UnsubscribeReactions()
        {
            base.UnsubscribeReactions();
            ActionSystem.UnsubscribeReaction<ApplyStatusGa>(ImmuneToStunReaction, ReactionTiming.PRE);
            ActionSystem.UnsubscribeReaction<DeathGA>(KillEnemyReaction, ReactionTiming.POST);
        }
        
        private void ImmuneToStunReaction(ApplyStatusGa applyStatusGa)
        {
            if (applyStatusGa.target == cardController && applyStatusGa.type == StatusType.Stun)
                applyStatusGa.NegateEffect();
        }
        
        private void KillEnemyReaction(DeathGA deathGa)
        {
            if (deathGa.killer == cardController)
            {
                foreach (CardMovement cardMovement in TargetingSystem.instance.RetrieveBoard(TargetType.Enemy))
                {
                    DealDamageGA passiveDamage = new DealDamageGA(ComputeCurrentDamage(spellData.damage), cardController, cardMovement.cardController);
                    ActionSystem.instance.AddReaction(passiveDamage);
                }
            }
        }
    }
}
