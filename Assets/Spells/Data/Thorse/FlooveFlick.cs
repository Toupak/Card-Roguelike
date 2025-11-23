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
        public override bool CanCastSpell()
        {
            if (CombatLoop.CombatLoop.instance == null || CombatLoop.CombatLoop.instance.currentTurn ==
                CombatLoop.CombatLoop.TurnType.Preparation)
                return false;

            if (cardController.cardStatus.IsStatusApplied(StatusType.Captured))
                return false;
            
            return !HasCastedThisTurn && EnergyController.instance.CheckForEnergy(1);
        }
        
        public override int ComputeEnergyCost()
        {
            return EnergyController.instance.currentEnergy;
        }

        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            
            int energy = EnergyController.instance.currentEnergy;
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
