using ActionReaction;
using ActionReaction.Game_Actions;

namespace Inventory.Items.Frames.data.Bersek
{
    public class BerserkFrameController : FrameController
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
            else if (dealDamageGa.attacker == cardController)
            {
                foreach (DealDamageGA.DamagePackage damagePackage in dealDamageGa.packages)
                {
                    damagePackage.amount += 1;
                }
            }
        }
    }
}
