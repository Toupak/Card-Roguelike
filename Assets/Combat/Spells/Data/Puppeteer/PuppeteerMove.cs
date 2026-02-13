using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Cards.Tween_Animations;
using UnityEngine;

namespace Combat.Spells.Data.Puppeteer
{
    public class PuppeteerMove : SpellController
    {
        [SerializeField] private SpellData singleSpell;

        public CardController puppetCard { get; set; }
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            yield return CardTween.NewPlaySelfAction(cardController);
            
            CardMovement target = targets[0];
            puppetCard.cardMovement.CurrentSlot.board.SendCardToOtherBoard(puppetCard.cardMovement.SlotIndex, target.tokenContainer);
            puppetCard.SetTokenParentController(target.cardController);
            cardController.rightButton.spellController.GetComponent<PuppeteerSwapMode>().UpdatePuppetMode(puppetCard);
        }
        
        protected override void SubscribeReactions()
        {
            base.SubscribeReactions();
            ActionSystem.SubscribeReaction<DeathGA>(DeathReaction, ReactionTiming.PRE);
        }

        protected override void UnsubscribeReactions()
        {
            base.UnsubscribeReactions();
            ActionSystem.UnsubscribeReaction<DeathGA>(DeathReaction, ReactionTiming.PRE);
        }

        private void DeathReaction(DeathGA deathGA)
        {
            if (deathGA.target != null && (deathGA.target == puppetCard || deathGA.target == puppetCard.tokenParentController))
                cardController.SetupSingleSpell(singleSpell);
        }
    }
}
