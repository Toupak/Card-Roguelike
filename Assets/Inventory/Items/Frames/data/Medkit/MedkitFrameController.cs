using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;

namespace Inventory.Items.Frames.data.Medkit
{
    public class MedkitFrameController : FrameController
    {
        public override void Setup(CardController controller, FrameData data)
        {
            base.Setup(controller, data);
            ActionSystem.SubscribeReaction<EndBattleGa>(EndBattleReaction, ReactionTiming.PRE);
        }

        public override void Remove()
        {
            ActionSystem.UnsubscribeReaction<EndBattleGa>(EndBattleReaction, ReactionTiming.PRE);
            base.Remove();
        }

        private void EndBattleReaction(EndBattleGa endBattleGa)
        {
            if (endBattleGa.isPlayerWin)
            {
                HealGa healGa = new HealGa(3, cardController, cardController);
                ActionSystem.instance.AddReaction(healGa);
            }
        }
    }
}
