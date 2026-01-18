using ActionReaction;
using ActionReaction.Game_Actions;

namespace Items.Frames.data.Bersek
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
            if (dealDamageGa.target == cardController || dealDamageGa.attacker == cardController)
                dealDamageGa.amount += 1;
        }
    }
}
