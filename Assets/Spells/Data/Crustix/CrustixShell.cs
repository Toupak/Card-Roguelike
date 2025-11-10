using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Spells.Data.Crustix
{
    public class CrustixShell : SpellController
    {
        [SerializeField] private Sprite regularSprite;
        [SerializeField] private Sprite armoredSprite;

        private bool isArmored;
        
        protected override void SubscribeReactions()
        {
            base.SubscribeReactions();
            ActionSystem.SubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.POST);
            ActionSystem.SubscribeReaction<ApplyStatusGa>(ApplyStatusReaction, ReactionTiming.POST);
            ActionSystem.SubscribeReaction<ConsumeStacksGa>(ConsumeStackReaction, ReactionTiming.POST);
        }

        protected override void UnsubscribeReactions()
        {
            base.UnsubscribeReactions();
            ActionSystem.UnsubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.POST);
            ActionSystem.UnsubscribeReaction<ApplyStatusGa>(ApplyStatusReaction, ReactionTiming.POST);
            ActionSystem.UnsubscribeReaction<ConsumeStacksGa>(ConsumeStackReaction, ReactionTiming.POST);
        }

        private void ConsumeStackReaction(ConsumeStacksGa consumeStacksGa)
        {
            if (consumeStacksGa.target == cardController && consumeStacksGa.type == StatusType.CrustixShell)
                UpdateArmorStatus();
        }

        private void ApplyStatusReaction(ApplyStatusGa applyStatusGa)
        {
            if (applyStatusGa.target == cardController && applyStatusGa.type == StatusType.CrustixShell)
                UpdateArmorStatus();
        }
        
        private void UpdateArmorStatus()
        {
            if (isArmored && !cardController.cardStatus.IsStatusApplied(StatusType.CrustixShell))
            {
                isArmored = false;
                cardController.SetArtwork(regularSprite);
            }
            else if (!isArmored && cardController.cardStatus.IsStatusApplied(StatusType.CrustixShell))
            {
                isArmored = true;
                cardController.SetArtwork(armoredSprite);
            }
        }

        private void DealDamageReaction(DealDamageGA dealDamageGa)
        {
            if (dealDamageGa.target == cardController && cardController.cardStatus.IsStatusApplied(spellData.inflictStatus))
            {
                DealDamageGA damageGa = new DealDamageGA(ComputeCurrentDamage(spellData.damage), cardController, dealDamageGa.attacker);
                ActionSystem.instance.AddReaction(damageGa);
                
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(spellData.inflictStatus, spellData.statusStacksApplied, cardController, cardController);
                ActionSystem.instance.AddReaction(applyStatusGa);
            }   
        }
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            ApplyStatusGa applyStatusGa = new ApplyStatusGa(spellData.inflictStatus, spellData.statusStacksApplied, cardController, cardController);
            ActionSystem.instance.Perform(applyStatusGa);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
        }
    }
}
