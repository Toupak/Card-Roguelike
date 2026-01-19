using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;

namespace Combat.Spells.Data.Rooster
{
    public class RoosterPassive : PassiveController
    {
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<ApplyStatusGa>(ApplyStatusReaction, ReactionTiming.POST);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<ApplyStatusGa>(ApplyStatusReaction, ReactionTiming.POST);
        }

        private void ApplyStatusReaction(ApplyStatusGa applyStatusGa)
        {
            if (applyStatusGa.target.cardMovement.IsEnemyCard && applyStatusGa.type == StatusType.Vengeance)
            {
                DealDamageGA dealDamageGa = new DealDamageGA(cardController.ComputeCurrentDamage(1), cardController, applyStatusGa.target);
                ActionSystem.instance.AddReaction(dealDamageGa);
            }
        }

        public override string ComputeTooltipDescription(int damage = int.MinValue)
        {
            return base.ComputeTooltipDescription(1);
        }
    }
}
