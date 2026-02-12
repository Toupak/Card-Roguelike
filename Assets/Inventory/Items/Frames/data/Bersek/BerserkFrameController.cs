using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;

namespace Inventory.Items.Frames.data.Bersek
{
    public class BerserkFrameController : FrameController
    {
        public override void Setup(CardController controller, FrameData data)
        {
            base.Setup(controller, data);
            controller.cardStats.IncreaseStat(CardStats.Stats.Strength, 1);
        }
        
        public override void Remove()
        {
            cardController.cardStats.DecreaseStat(CardStats.Stats.Strength, 1);
            base.Remove();
        }
        
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
            DealDamageGA.DamagePackage package = dealDamageGa.GetDamagePackageForTarget(cardController);
            
            if (package != null)
                package.amount += 1;
        }
    }
}
