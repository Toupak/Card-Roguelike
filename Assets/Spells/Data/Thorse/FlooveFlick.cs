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
            return !HasCastedThisTurn && EnergyController.instance.currentEnergyCount > 0;
        }
        
        protected override void ConsumeEnergy()
        {
        }
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            
            int energy = EnergyController.instance.currentEnergyCount;
            EnergyController.instance.RemoveEnergy(energy);

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
