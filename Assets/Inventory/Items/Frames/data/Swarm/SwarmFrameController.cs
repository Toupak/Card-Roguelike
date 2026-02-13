using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;

namespace Inventory.Items.Frames.data.Swarm
{
    public class SwarmFrameController : FrameController
    {
        public override void Setup(CardController controller, FrameData data)
        {
            base.Setup(controller, data);
            ActionSystem.SubscribeReaction<SpawnCardGA>(SpawnCardReaction, ReactionTiming.POST);
        }

        public override void Remove()
        {
            ActionSystem.UnsubscribeReaction<SpawnCardGA>(SpawnCardReaction, ReactionTiming.POST);
            base.Remove();
        }

        private void SpawnCardReaction(SpawnCardGA spawnCardGa)
        {
            if (spawnCardGa.isToken && spawnCardGa.spawner != null && spawnCardGa.spawner == cardController)
            {
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.PermanentBonusDamage, 1, cardController, spawnCardGa.spawnedCard);
                ActionSystem.instance.AddReaction(applyStatusGa);
            }
        }
    }
}
