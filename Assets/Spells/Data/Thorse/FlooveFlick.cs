using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using CombatLoop.EnergyBar;
using UnityEngine;

namespace Spells.Data.Thorse
{
    public class FlooveFlick : SpellController
    {
        public override void Setup(CardController controller, SpellData data, SpellButton attacheSpellButton, SpellButton otherSpell)
        {
            base.Setup(controller, data, attacheSpellButton, otherSpell);
            thisSpellButton.UpdateTooltipEnergyCost(EnergyController.instance != null ? EnergyController.instance.StartingEnergyCount : 3);
        }
        
        protected override void SubscribeReactions()
        {
            base.SubscribeReactions();
            ActionSystem.SubscribeReaction<ConsumeEnergyGa>(ConsumeEnergyReaction, ReactionTiming.POST);
        }

        protected override void UnsubscribeReactions()
        {
            base.UnsubscribeReactions();
            ActionSystem.UnsubscribeReaction<ConsumeEnergyGa>(ConsumeEnergyReaction, ReactionTiming.POST);
        }

        private void ConsumeEnergyReaction(ConsumeEnergyGa consumeEnergyGa)
        {
            thisSpellButton.UpdateTooltipEnergyCost(EnergyController.instance.currentEnergyCount);
        }

        protected override void EndTurnRefreshCooldownReaction(StartTurnGa startTurnGa)
        {
            base.EndTurnRefreshCooldownReaction(startTurnGa);
            thisSpellButton.UpdateTooltipEnergyCost(EnergyController.instance.StartingEnergyCount);
        }
        
        public override bool CanCastSpell()
        {
            if (CombatLoop.CombatLoop.instance == null || CombatLoop.CombatLoop.instance.currentTurn ==
                CombatLoop.CombatLoop.TurnType.Preparation)
                return false;

            if (cardController.cardStatus.IsStatusApplied(StatusType.Captured))
                return false;
            
            return !HasCastedThisTurn && EnergyController.instance.currentEnergyCount > 0;
        }

        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            
            int energy = EnergyController.instance.currentEnergyCount;
            yield return ConsumeEnergy(energy);

            int damage = ComputeCurrentDamage(energy * spellData.damage);

            foreach (CardMovement cardMovement in targets)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                DealDamageGA flick = new DealDamageGA(damage, cardController, cardMovement.cardController);
                ActionSystem.instance.Perform(flick);
            }
        }
    }
}
