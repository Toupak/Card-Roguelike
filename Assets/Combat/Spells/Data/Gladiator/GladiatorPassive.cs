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
            if (dealDamageGa.attacker != null && dealDamageGa.target != null && dealDamageGa.attacker == cardController && !dealDamageGa.isDamageNegated && dealDamageGa.amount > 0)
            {
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Spear, 1, cardController, dealDamageGa.target);
                ActionSystem.instance.AddReaction(applyStatusGa);
            }
            else if (dealDamageGa.attacker != null && dealDamageGa.target != null && dealDamageGa.target == cardController && dealDamageGa.attacker.cardStatus.IsStatusApplied(StatusType.Spear))
            {
                DealDamageGA spearDamage = new DealDamageGA(dealDamageGa.attacker.cardStatus.GetCurrentStackCount(StatusType.Spear), cardController, dealDamageGa.attacker);
                ActionSystem.instance.AddReaction(spearDamage);
            }
        }
    }
}
