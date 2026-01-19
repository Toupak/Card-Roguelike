using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;
using UnityEngine;

namespace Combat.Spells.Data.Gumball
{
    public class GumballArmor : PassiveController
    {
        [SerializeField] private Sprite gumballStrong;
        [SerializeField] private Sprite gumballWeak;

        private bool isStrong = true;
        
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<DealDamageGA>(ArmorReaction, ReactionTiming.PRE);
            ActionSystem.SubscribeReaction<ApplyStatusGa>(ApplyStackReaction, ReactionTiming.POST);
            ActionSystem.SubscribeReaction<ConsumeStacksGa>(ConsumeStackReaction, ReactionTiming.POST);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<DealDamageGA>(ArmorReaction, ReactionTiming.PRE);
            ActionSystem.UnsubscribeReaction<ApplyStatusGa>(ApplyStackReaction, ReactionTiming.POST);
            ActionSystem.UnsubscribeReaction<ConsumeStacksGa>(ConsumeStackReaction, ReactionTiming.POST);
        }

        private void ArmorReaction(DealDamageGA dealDamageGa)
        {
            if (dealDamageGa.target == cardController && cardController.cardStatus.IsStatusApplied(StatusType.Stun))
            {
                dealDamageGa.NegateDamage();
                
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.GumBoom, 1, dealDamageGa.attacker, cardController);
                ActionSystem.instance.AddReaction(applyStatusGa);
            }
        }
        
        private void ApplyStackReaction(ApplyStatusGa applyStatusGa)
        {
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
