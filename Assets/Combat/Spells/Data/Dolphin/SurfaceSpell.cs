using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Spells.Data.Dolphin
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

            if (targets.Count < 1)
                yield break;
            
            DealDamageGA dealDamageGa = new DealDamageGA(ComputeCurrentDamage(spellData.damage), cardController, targets[Random.Range(0, targets.Count)].cardController);
            ActionSystem.instance.Perform(dealDamageGa);
            
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
            if (startTurnGa.starting == CombatLoop.CombatLoop.TurnType.Player)
            {
                if (cardController.cardData.spellList.Count == 1)
                    cardController.SetupSingleSpell(diveSpell);
                else
                    cardController.SetupRightSpell(diveSpell);
            }
        }
    }
}
