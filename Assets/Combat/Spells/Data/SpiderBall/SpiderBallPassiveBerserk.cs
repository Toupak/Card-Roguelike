using ActionReaction;
using ActionReaction.Game_Actions;
using BoomLib.Tools;
using Passives;

namespace Spells.Data.SpiderBall
{
    public class SpiderBallPassiveBerserk : PassiveController
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
                dealDamageGa.amount *= 2;
            else if (dealDamageGa.attacker == cardController && Tools.RandomBool())
                dealDamageGa.amount *= 2;
        }
    }
}
