using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Passives;

namespace Spells.Data.GnomeCarrier
{
    public class TokenProtectsParent : PassiveController
    {
        public override void Setup(CardController controller, PassiveData data, int index)
        {
            base.Setup(controller, data, index);

            ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.PermanentProtected, 1, cardController, controller.tokenParentController);
            
            if (ActionSystem.instance.IsPerforming)
                ActionSystem.instance.AddReaction(applyStatusGa);
            else
                ActionSystem.instance.Perform(applyStatusGa);
        }

        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.PRE, 100);
            ActionSystem.SubscribeReaction<DeathGA>(DeathReaction, ReactionTiming.PRE);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.PRE);
            ActionSystem.UnsubscribeReaction<DeathGA>(DeathReaction, ReactionTiming.PRE);
        }

        private void DealDamageReaction(DealDamageGA dealDamageGa)
        {
            if (dealDamageGa.target != null && IsCardProtected(dealDamageGa.target))
                dealDamageGa.SwitchTarget(cardController);
        }
        
        private bool IsCardProtected(CardController card)
        {
            return card != null && card == cardController.tokenParentController;
        }
        
        private void DeathReaction(DeathGA deathGa)
        {
            if (deathGa.target == cardController)
            {
                ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.PermanentProtected, 1, cardController.tokenParentController, cardController.tokenParentController);
                ActionSystem.instance.AddReaction(consumeStacksGa);
            }
        }
    }
}
