using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.EnergyBar;
using UnityEngine;

namespace Combat.Spells.Data.Turtle
{
    public class TurtleReceive : SpellController
    {
        public override bool CanCastSpell()
        {
            if (HasCastedThisTurn)
                return false;

            if (CombatLoop.instance == null || CombatLoop.instance.currentTurn ==
                CombatLoop.TurnType.Preparation)
                return false;
            
            if (!EnergyController.instance.CheckForEnergy(spellData.energyCost))
                return false;

            return cardController.cardStatus.IsStatusApplied(StatusType.Stun) && !cardController.cardStatus.IsStatusApplied(StatusType.Captured);
        }
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            int stacks = cardController.cardStatus.GetCurrentStackCount(StatusType.Stun);
            
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
