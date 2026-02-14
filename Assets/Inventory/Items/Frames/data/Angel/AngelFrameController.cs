using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;

namespace Inventory.Items.Frames.data.Angel
{
    public class AngelFrameController : FrameController
    {
        private bool hasBeenUsed;
        
        public override void Setup(CardController controller, FrameData data)
        {
            base.Setup(controller, data);
            ActionSystem.SubscribeReaction<DeathGA>(DeathReaction, ReactionTiming.PRE);
        }

        public override void Remove()
        {
            ActionSystem.UnsubscribeReaction<DeathGA>(DeathReaction, ReactionTiming.PRE);
            base.Remove();
        }
        
        private void DeathReaction(DeathGA deathGa)
        {
            if (!hasBeenUsed && deathGa.target == cardController)
            {
                hasBeenUsed = true;
                deathGa.PreventDeath();
                
                cardController.cardHealth.SetHealth(1);
            }
        }
    }
}
