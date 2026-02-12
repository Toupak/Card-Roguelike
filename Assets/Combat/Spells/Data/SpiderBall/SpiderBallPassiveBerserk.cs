using ActionReaction;
using ActionReaction.Game_Actions;
using BoomLib.Tools;
using Combat.Passives;

namespace Combat.Spells.Data.SpiderBall
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
            DealDamageGA.DamagePackage package = dealDamageGa.GetPackageFromTarget(cardController);
            
            if (package != null && Tools.RandomBool())
                package.amount *= 2;
            else if (dealDamageGa.attacker == cardController && Tools.RandomBool())
            {
                foreach (DealDamageGA.DamagePackage damagePackage in dealDamageGa.packages)
                {
                    damagePackage.amount *= 2;
                }
            }
        }
    }
}
