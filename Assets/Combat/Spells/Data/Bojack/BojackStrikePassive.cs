using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;

namespace Combat.Spells.Data.Bojack
{
    public class BojackStrikePassive : PassiveController
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
            if (applyStatusGa.type == StatusType.Moonlight && applyStatusGa.target != null)
            {
                int stackCount = applyStatusGa.target.cardStatus.GetCurrentStackCount(StatusType.Moonlight);

                if (stackCount >= 3)
                {
                    ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.Moonlight, stackCount, null, applyStatusGa.target);
                    ActionSystem.instance.AddReaction(consumeStacksGa);

                    DealDamageGA dealDamageGa = new DealDamageGA(3, null, applyStatusGa.target);
                    ActionSystem.instance.AddReaction(dealDamageGa);
                }
            }
        }
    }
}
