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
            ActionSystem.SubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.POST);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.POST);
        }

        private void DealDamageReaction(DealDamageGA dealDamageGa)
        {
            if (dealDamageGa.target != null && dealDamageGa.attacker != null && dealDamageGa.attacker == cardController && !dealDamageGa.isDamageNegated && dealDamageGa.amount > 0)
            {
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Moonlight, 1, cardController, dealDamageGa.target);
                ActionSystem.instance.AddReaction(applyStatusGa);
            }
        }
    }
}
