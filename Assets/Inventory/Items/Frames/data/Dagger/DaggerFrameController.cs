using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat;
using Combat.Spells;
using Combat.Spells.Targeting;

namespace Inventory.Items.Frames.data.Dagger
{
    public class DaggerFrameController : FrameController
    {
        public override void Setup(CardController controller, FrameData data)
        {
            base.Setup(controller, data);
            ActionSystem.SubscribeReaction<StartTurnGa>(StartTurnReaction, ReactionTiming.POST);
        }

        public override void Remove()
        {
            ActionSystem.UnsubscribeReaction<StartTurnGa>(StartTurnReaction, ReactionTiming.POST);
            base.Remove();
        }

        private void StartTurnReaction(StartTurnGa startTurnGa)
        {
            if (startTurnGa.starting == CombatLoop.TurnType.Player)
            {
                CardMovement target = TargetingSystem.instance.RetrieveRandomCard(TargetType.Enemy);

                if (target != null)
                {
                    DealDamageGA dealDamageGa = new DealDamageGA(cardController.ComputeCurrentDamage(1), cardController, target.cardController);
                    ActionSystem.instance.AddReaction(dealDamageGa);
                }
            }
        }
    }
}
