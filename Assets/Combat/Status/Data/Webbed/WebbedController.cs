using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;

namespace Combat.Status.Data.Webbed
{
    public class WebbedController : StatusController
    {
        public override void AddStack(int amount)
        {
            int stackCount = GetStackCount();
            
            if (stackCount >= 2)
            {
                ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.Webbed, stackCount, cardController, cardController);
                ActionSystem.instance.AddReaction(consumeStacksGa);
                
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Stun, 1, cardController, cardController);
                ActionSystem.instance.AddReaction(applyStatusGa);
            }
        }
    }
}
