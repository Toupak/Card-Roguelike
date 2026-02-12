using ActionReaction;
using ActionReaction.Game_Actions;
using BoomLib.Tools;
using Combat.Passives;

namespace Combat.Spells.Data.SpiderBall
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
            DealDamageGA.DamagePackage package = dealDamageGa.GetPackageFromTarget(cardController);
            
            if (package != null && Tools.RandomBool())
                dealDamageGa.NegateDamage(package);
        }
    }
}
