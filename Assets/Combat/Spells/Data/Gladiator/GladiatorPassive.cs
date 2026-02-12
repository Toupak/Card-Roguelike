using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;

namespace Combat.Spells.Data.Gladiator
{
    public class GladiatorPassive : PassiveController
    {
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.POST);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.POST);
        }

        private void DealDamageReaction(DealDamageGA dealDamageGa)
        {
            if (dealDamageGa.attacker != null && dealDamageGa.attacker == cardController)
            {
                foreach (DealDamageGA.DamagePackage package in dealDamageGa.packages)
                {
                    if (!package.isDamageNegated && package.amount > 0)
                    {
                        ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Spear, 1, cardController, package.target);
                        ActionSystem.instance.AddReaction(applyStatusGa);
                    }
                }
            }
            else if (dealDamageGa.attacker != null && dealDamageGa.IsCardTargeted(cardController))
            {
                int spearCount = dealDamageGa.attacker.cardStatus.GetCurrentStackCount(StatusType.Spear);

                if (spearCount > 0)
                {
                    DealDamageGA returnDamage = new DealDamageGA(spearCount, cardController, dealDamageGa.attacker);
                    ActionSystem.instance.AddReaction(returnDamage);
                }
            }
        }
    }
}
