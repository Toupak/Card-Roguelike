using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;

namespace Combat.Spells.Data.Frien
{
    public class FrienWeakBuff : PassiveController
    {
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<DealDamageGA>(BuffDamageReaction, ReactionTiming.PRE);
        }

        private void OnDisable()
        {
            ActionSystem.SubscribeReaction<DealDamageGA>(BuffDamageReaction, ReactionTiming.PRE);
        }

        private void BuffDamageReaction(DealDamageGA dealDamageGa)
        {
            if (dealDamageGa.attacker == cardController && cardController.cardStatus.IsStatusApplied(StatusType.Weak))
            {
                int damageBonus = cardController.cardStatus.GetCurrentStackCount(StatusType.Weak) * 3;
                foreach (DealDamageGA.DamagePackage package in dealDamageGa.packages)
                {
                    package.amount += damageBonus;
                }
            }
        }
    }
}
