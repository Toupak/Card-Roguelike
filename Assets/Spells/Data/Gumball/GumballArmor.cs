using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Status;

namespace Spells.Data.Gumball
{
    public class GumballArmor : SpellController
    {
        protected override void SubscribeReactions()
        {
            base.SubscribeReactions();
            ActionSystem.SubscribeReaction<DealDamageGA>(ArmorReaction, ReactionTiming.PRE);
            ActionSystem.SubscribeReaction<ApplyStatusGa>(ApplyStackReaction, ReactionTiming.POST);
        }

        protected override void UnsubscribeReactions()
        {
            base.UnsubscribeReactions();
            ActionSystem.UnsubscribeReaction<DealDamageGA>(ArmorReaction, ReactionTiming.PRE);
            ActionSystem.UnsubscribeReaction<ApplyStatusGa>(ApplyStackReaction, ReactionTiming.POST);
        }
        
        private void ArmorReaction(DealDamageGA dealDamageGa)
        {
            if (dealDamageGa.target == cardController && cardController.cardStatus.IsStatusApplied(StatusType.Stun))
            {
                dealDamageGa.NegateDamage();
                
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(spellData.inflictStatus, spellData.statusStacksApplied, dealDamageGa.attacker, cardController);
                ActionSystem.instance.AddReaction(applyStatusGa);
            }
        }
        
        private void ApplyStackReaction(ApplyStatusGa applyStatusGa)
        {
            if (applyStatusGa.target == cardController && applyStatusGa.type == spellData.inflictStatus && cardController.cardStatus.IsStatusApplied(spellData.inflictStatus))
            {
                int currentStacks = cardController.cardStatus.currentStacks[spellData.inflictStatus];
                int maxStacks = StatusSystem.instance.GetStatusData(spellData.inflictStatus).maxStackCount;

                if (currentStacks >= maxStacks)
                {
                    DeathGA deathGa = new DeathGA(applyStatusGa.attacker, cardController);
                    ActionSystem.instance.AddReaction(deathGa);
                }
            }
        }
    }
}
