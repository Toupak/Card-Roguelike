using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Passives;

namespace Spells.Data.Pillar_Man
{
    public class PillarTaunt : PassiveController
    {
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<ApplyStatusGa>(ApplyStatusReaction, ReactionTiming.PRE);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<ApplyStatusGa>(ApplyStatusReaction, ReactionTiming.PRE);
        }
        
        private void ApplyStatusReaction(ApplyStatusGa applyStatusGa)
        {
            if (applyStatusGa.type == StatusType.Taunt)
                applyStatusGa.target = cardController;
        }
    }
}
