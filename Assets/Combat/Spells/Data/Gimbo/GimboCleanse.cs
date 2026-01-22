using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.EnergyBar;
using UnityEngine;

namespace Combat.Spells.Data.Gimbo
{
    public class GimboCleanse : SpellController
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

            if (cardController.cardStatus.IsStatusApplied(StatusType.Captured))
                return false;

            return cardController.cardStatus.IsStatusApplied(StatusType.Stun);
        }
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.Stun, cardController.cardStatus.GetCurrentStackCount(StatusType.Stun), cardController, cardController);
            ActionSystem.instance.Perform(consumeStacksGa);
            
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            ApplyStatusGa applyStatusGa = new ApplyStatusGa(spellData.inflictStatus, spellData.statusStacksApplied, cardController, cardController);
            ActionSystem.instance.Perform(applyStatusGa);
        }
    }
}
