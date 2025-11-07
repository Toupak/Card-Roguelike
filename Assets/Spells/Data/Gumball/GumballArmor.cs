using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Status;
using UnityEngine;

namespace Spells.Data.Gumball
{
    public class GumballArmor : SpellController
    {
        [SerializeField] private Sprite gumballStrong;
        [SerializeField] private Sprite gumballWeak;

        private bool isStrong = true;
        
        protected override void SubscribeReactions()
        {
            base.SubscribeReactions();
            ActionSystem.SubscribeReaction<DealDamageGA>(ArmorReaction, ReactionTiming.PRE);
            ActionSystem.SubscribeReaction<ApplyStatusGa>(ApplyStackReaction, ReactionTiming.POST);
            ActionSystem.SubscribeReaction<ConsumeStacksGa>(ConsumeStackReaction, ReactionTiming.POST);
        }

        protected override void UnsubscribeReactions()
        {
            base.UnsubscribeReactions();
            ActionSystem.UnsubscribeReaction<DealDamageGA>(ArmorReaction, ReactionTiming.PRE);
            ActionSystem.UnsubscribeReaction<ApplyStatusGa>(ApplyStackReaction, ReactionTiming.POST);
            ActionSystem.UnsubscribeReaction<ConsumeStacksGa>(ConsumeStackReaction, ReactionTiming.POST);
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
            if (applyStatusGa.target == cardController && applyStatusGa.type == StatusType.GumBoom && cardController.cardStatus.IsStatusApplied(StatusType.GumBoom))
            {
                int currentStacks = cardController.cardStatus.currentStacks[spellData.inflictStatus];
                int maxStacks = StatusSystem.instance.GetStatusData(spellData.inflictStatus).maxStackCount;

                if (currentStacks >= maxStacks)
                {
                    DeathGA deathGa = new DeathGA(applyStatusGa.attacker, cardController);
                    ActionSystem.instance.AddReaction(deathGa);
                }
            }

            if (applyStatusGa.target == cardController && applyStatusGa.type == StatusType.Stun && isStrong)
            {
                isStrong = false;
                UpdateGumballArtwork();
            }
        }
        
        private void ConsumeStackReaction(ConsumeStacksGa consumeStacksGa)
        {
            if (consumeStacksGa.target == cardController && consumeStacksGa.type == StatusType.Stun && !isStrong && !cardController.cardStatus.IsStatusApplied(StatusType.Stun))
            {
                isStrong = true;
                UpdateGumballArtwork();
            }
        }

        private void UpdateGumballArtwork()
        {
            cardController.SetArtwork(isStrong ? gumballStrong : gumballWeak);
        }
    }
}
