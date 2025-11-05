using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Spells.Targeting;
using UnityEngine;

namespace Spells.Data.Monke
{
    public class GoBackToMonke : SpellController
    {
        private bool isAloneOnBoard => cardController.cardMovement.CurrentSlot.board.Slots.Count == 1;
        
        protected override IEnumerator SelectTargetAndCast(Transform startPosition)
        {
            bool isAlone = isAloneOnBoard;

            TargetingMode targetingMode = isAlone ? TargetingMode.All : spellData.targetingMode;
            
            yield return TargetingSystem.instance.SelectTargets(cardController.cardMovement, startPosition, spellData.targetType, targetingMode, spellData.targetCount);
            if (TargetingSystem.instance.IsCanceled)
                CancelTargeting();
            else
                yield return CastSpellOnTarget(TargetingSystem.instance.Targets);
        }
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            int damage = spellData.damage;

            if (isAloneOnBoard)
                damage += 1;
            
            foreach (CardMovement target in targets)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                DealDamageGA dealDamage = new DealDamageGA(ComputeCurrentDamage(damage), cardController, target.cardController);
                ActionSystem.instance.Perform(dealDamage);
                
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                ActionSystem.instance.Perform(dealDamage);
            }
        }
    }
}
