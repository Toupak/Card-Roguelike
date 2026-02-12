using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;

namespace Combat.Spells.Data.Racoon
{
    public class RacoonPassive : PassiveController
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
            if (dealDamageGa.GetPackageFromTarget(cardController) != null && dealDamageGa.attacker != null && dealDamageGa.attacker.cardStatus.IsStatusApplied(StatusType.RacoonLastTarget))
            {
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Fury, 1, cardController, cardController);
                ActionSystem.instance.AddReaction(applyStatusGa);
            }
        }
    }
}
