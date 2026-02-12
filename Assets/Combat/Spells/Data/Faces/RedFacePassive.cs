using ActionReaction;
using ActionReaction.Game_Actions;
using Combat.Passives;

namespace Combat.Spells.Data.Faces
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
            DealDamageGA.DamagePackage package = dealDamageGa.GetDamagePackageForTarget(cardController);
            
            if (package != null && cardController.cardMovement.tokenContainer.slotCount > 0)
                dealDamageGa.SwitchTarget(package, cardController.cardMovement.tokenContainer.Slots[0].CurrentCard.cardController);
        }
    }
}
