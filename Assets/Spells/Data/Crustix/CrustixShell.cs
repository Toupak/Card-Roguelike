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
            ActionSystem.SubscribeReaction<ApplyStatusGa>(ApplyStatusReaction, ReactionTiming.POST);
            ActionSystem.SubscribeReaction<ConsumeStacksGa>(ConsumeStackReaction, ReactionTiming.POST);
            ActionSystem.SubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.POST);
        }

        protected override void UnsubscribeReactions()
        {
            base.UnsubscribeReactions();
            ActionSystem.UnsubscribeReaction<ApplyStatusGa>(ApplyStatusReaction, ReactionTiming.POST);
            ActionSystem.UnsubscribeReaction<ConsumeStacksGa>(ConsumeStackReaction, ReactionTiming.POST);
            ActionSystem.UnsubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.POST);
        }

        private void DealDamageReaction(DealDamageGA dealDamageGa)
        {
            if (dealDamageGa.attacker == cardController && isArmored)
            {
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.ReturnDamage, 1, cardController, cardController);
                ActionSystem.instance.AddReaction(applyStatusGa);
            }
        }

        private void ConsumeStackReaction(ConsumeStacksGa consumeStacksGa)
        {
            if (consumeStacksGa.target == cardController && consumeStacksGa.type == StatusType.ReturnDamage)
                UpdateArmorStatus();
        }

        private void ApplyStatusReaction(ApplyStatusGa applyStatusGa)
        {
            if (applyStatusGa.target == cardController && applyStatusGa.type == StatusType.ReturnDamage)
                UpdateArmorStatus();
        }
        
        private void UpdateArmorStatus()
        {
            if (isArmored && !cardController.cardStatus.IsStatusApplied(StatusType.ReturnDamage))
            {
                isArmored = false;
                cardController.SetArtwork(regularSprite);
            }
            else if (!isArmored && cardController.cardStatus.IsStatusApplied(StatusType.ReturnDamage))
            {
                isArmored = true;
                cardController.SetArtwork(armoredSprite);
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
