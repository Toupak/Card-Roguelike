using ActionReaction;
using ActionReaction.Game_Actions;

namespace Spells.Data.Gumball
{
    public class GumballArmor : SpellController
    {
        protected override void SubscribeReactions()
        {
            base.SubscribeReactions();
            ActionSystem.SubscribeReaction<DealDamageGA>(ArmorReaction, ReactionTiming.PRE);
        }
        
        protected override void UnsubscribeReactions()
        {
            base.UnsubscribeReactions();
            ActionSystem.UnsubscribeReaction<DealDamageGA>(ArmorReaction, ReactionTiming.PRE);
        }
        
        private void ArmorReaction(DealDamageGA dealDamageGa)
        {
            if (dealDamageGa.target == cardController && cardController.cardStatus.IsStun)
                dealDamageGa.NegateDamage();
        }
    }
}
