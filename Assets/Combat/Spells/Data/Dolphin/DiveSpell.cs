using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.Spells.Data.Dolphin
{
    public class DiveSpell : SpellController
    {
        [SerializeField] private SpellData surfaceSpell;

        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Dive, 1, cardController, cardController);
            ActionSystem.instance.Perform(applyStatusGa);
        }
        
        protected override void SubscribeReactions()
        {
            base.SubscribeReactions();
            ActionSystem.SubscribeReaction<ConsumeStacksGa>(ConsumeStacksReaction, ReactionTiming.POST);
        }

        protected override void UnsubscribeReactions()
        {
            base.UnsubscribeReactions();
            ActionSystem.UnsubscribeReaction<ConsumeStacksGa>(ConsumeStacksReaction, ReactionTiming.POST);
        }

        private void ConsumeStacksReaction(ConsumeStacksGa consumeStacksGa)
        {
            if (consumeStacksGa.target == cardController && consumeStacksGa.type == StatusType.Dive)
            {
                if (cardController.cardData.spellList.Count == 1)
                    cardController.SetupSingleSpell(surfaceSpell);
                else
                    cardController.SetupRightSpell(surfaceSpell);
            }
        }
    }
}
