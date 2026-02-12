using Cards.Scripts;

namespace Inventory.Items.Frames.data.Sword
{
    public class SwordFrameController : FrameController
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
    }
}
