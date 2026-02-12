using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;

namespace Combat.Spells.Data.Guard
{
    public class GuardPassive : PassiveController
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
            if (dealDamageGa.GetPackageFromTarget(cardController) != null)
            {
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Vengeance, 1, cardController, dealDamageGa.attacker);
                ActionSystem.instance.AddReaction(applyStatusGa);
            }
        }
    }
}
