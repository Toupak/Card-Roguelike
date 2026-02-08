using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.Spells.Data.Dolphin
{
    public class SurfaceSpell : SpellController
    {
        [SerializeField] private SpellData diveSpell;
        
        public override void Setup(CardController controller, SpellData data, SpellButton attacheSpellButton, SpellButton otherSpell)
        {
            base.Setup(controller, data, attacheSpellButton, otherSpell);
            SetShinyState(true);
        }
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            if (targets.Count < 1)
                yield break;

            List<CardMovement> sonarTargets = targets.Where((c) => c.cardController.cardStatus.IsStatusApplied(StatusType.Sonar)).ToList();
            if (sonarTargets.Count > 0)
                targets = sonarTargets;

            CardController target = targets[Random.Range(0, targets.Count)].cardController;
            
            DealDamageGA dealDamageGa = new DealDamageGA(ComputeCurrentDamage(spellData.damage), cardController, target);
            ActionSystem.instance.Perform(dealDamageGa);

            if (target.cardStatus.IsStatusApplied(StatusType.Sonar))
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.Sonar, 1, cardController, target);
                ActionSystem.instance.Perform(consumeStacksGa);
            }
            
            SetShinyState(false);
        }
        
        protected override void SubscribeReactions()
        {
            base.SubscribeReactions();
            ActionSystem.SubscribeReaction<StartTurnGa>(StartTurnReaction, ReactionTiming.PRE);
        }

        protected override void UnsubscribeReactions()
        {
            base.UnsubscribeReactions();
            ActionSystem.UnsubscribeReaction<StartTurnGa>(StartTurnReaction, ReactionTiming.PRE);
        }

        private void StartTurnReaction(StartTurnGa startTurnGa)
        {
            if (startTurnGa.starting == CombatLoop.TurnType.Player)
            {
                if (cardController.cardData.spellList.Count == 1)
                    cardController.SetupSingleSpell(diveSpell);
                else
                    cardController.SetupRightSpell(diveSpell);
            }
        }
    }
}
