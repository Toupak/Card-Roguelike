using ActionReaction;
using ActionReaction.Game_Actions;
using BoomLib.Tools;
using Passives;

namespace Spells.Data.SpiderBall
{
    public class SpiderBallPassiveDodge : PassiveController
    {
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.PRE);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.PRE);
        }

        private void DealDamageReaction(DealDamageGA dealDamageGa)
        {
            if (dealDamageGa.target == cardController && Tools.RandomBool())
                dealDamageGa.NegateDamage();
        }
    }
}
