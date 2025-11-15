using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using Cards.Scripts;
using CombatLoop.EnergyBar;
using UnityEngine;

namespace Spells.Data.Gimbo
{
    public class GimboCleanse : SpellController
    {
        public override bool CanCastSpell()
        {
            if (HasCastedThisTurn)
                return false;

            if (CombatLoop.CombatLoop.instance == null || CombatLoop.CombatLoop.instance.currentTurn ==
                CombatLoop.CombatLoop.TurnType.Preparation)
                return false;
            
            if (!EnergyController.instance.CheckForEnergy(spellData.energyCost))
                return false;

            if (cardController.cardStatus.IsStatusApplied(StatusType.Captured))
                return false;

            return cardController.cardStatus.IsStatusApplied(StatusType.Stun);
        }
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
        }
    }
}
