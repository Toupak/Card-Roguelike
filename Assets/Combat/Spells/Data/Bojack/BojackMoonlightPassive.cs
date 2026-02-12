using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;

namespace Combat.Spells.Data.Bojack
{
    public class BojackMoonlightPassive : PassiveController
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
            if (dealDamageGa.attacker != null && dealDamageGa.attacker == cardController)
            {
                foreach (DealDamageGA.DamagePackage damagePackage in dealDamageGa.packages)
                {
                    if (!damagePackage.isDamageNegated && damagePackage.amount > 0)
                    {
                        ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Moonlight, 1, null, damagePackage.target);
                        ActionSystem.instance.AddReaction(applyStatusGa);
                    }
                }
            }
        }
    }
}
