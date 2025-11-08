using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using CombatLoop.EnergyBar;
using UnityEngine;

namespace Spells.Data.Turtle
{
    public class TurtleReceive : SpellController
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

            return cardController.cardStatus.IsStatusApplied(StatusType.Stun);
        }
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            int stacks = cardController.cardStatus.IsStatusApplied(StatusType.Stun)
                ? cardController.cardStatus.currentStacks[StatusType.Stun]
                : 0;
            
            ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.Stun, stacks, cardController, cardController);
            ActionSystem.instance.Perform(consumeStacksGa);
            
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            
            foreach (CardMovement target in targets)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                DealDamageGA dealDamageGa = new DealDamageGA(stacks, cardController, target.cardController);
                ActionSystem.instance.Perform(dealDamageGa);
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            }
        }
    }
}
