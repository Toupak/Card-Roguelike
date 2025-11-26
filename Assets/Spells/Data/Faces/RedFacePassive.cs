using ActionReaction;
using ActionReaction.Game_Actions;
using Passives;

namespace Spells.Data.Faces
{
    public class RedFacePassive : PassiveController
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
            if (dealDamageGa.target == cardController && cardController.cardMovement.tokenContainer.slotCount > 0)
                dealDamageGa.SwitchTarget(cardController.cardMovement.tokenContainer.Slots[0].CurrentCard.cardController);
        }
    }
}
