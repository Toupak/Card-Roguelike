using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;

namespace Combat.Status.Data.Stagger
{
    public class StaggerController : StatusController
    {
        public override void AddStack(int amount)
        {
            int stackCount = GetStackCount();
            
            if (stackCount >= 3)
            {
                ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.Stagger, stackCount, cardController, cardController);
                ActionSystem.instance.AddReaction(consumeStacksGa);
                
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Stun, 1, cardController, cardController);
                ActionSystem.instance.AddReaction(applyStatusGa);
            }
        }
    }
}
