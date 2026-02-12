using ActionReaction;
using ActionReaction.Game_Actions;

namespace Inventory.Items.Frames.data.Sword
{
    public class SwordFrameController : FrameController
    {
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.PRE);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.PRE);
        }
    
        private void DealDamageReaction(DealDamageGA dealDamageGa)
        {
            DealDamageGA.DamagePackage package = dealDamageGa.GetPackageFromTarget(cardController);
            
            if (package != null)
                package.amount += 1;
        }
    }
}
