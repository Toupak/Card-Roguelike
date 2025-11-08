using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Spells.Passives;

namespace Spells.Data.Thorse
{
    public class ImmuneToStunPassive : PassiveController
    {
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<ApplyStatusGa>(ImmuneToStunReaction, ReactionTiming.PRE);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<ApplyStatusGa>(ImmuneToStunReaction, ReactionTiming.PRE);
        }
        
        private void ImmuneToStunReaction(ApplyStatusGa applyStatusGa)
        {
            if (applyStatusGa.target == cardController && applyStatusGa.type == StatusType.Stun)
                applyStatusGa.NegateEffect();
        }    
    }
}
