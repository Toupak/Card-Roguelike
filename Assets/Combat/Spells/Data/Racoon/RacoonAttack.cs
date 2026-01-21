using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Spells.Targeting;
using UnityEngine;

namespace Combat.Spells.Data.Racoon
{
    public class RacoonAttack : SpellController
    {
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            List<CardMovement> allEnemies = TargetingSystem.instance.RetrieveBoard(TargetType.Enemy);

            foreach (CardMovement cardMovement in allEnemies)
            {            
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                if (targets.Contains(cardMovement))
                    yield return AttackTarget(cardMovement);
                else
                    yield return ClearStacks(cardMovement);
            }
        }

        private IEnumerator AttackTarget(CardMovement cardMovement)
        {
            bool isLastTarget = cardMovement.cardController.cardStatus.IsStatusApplied(StatusType.RacoonLastTarget);

            DealDamageGA dealDamageGa = new DealDamageGA(ComputeCurrentDamage(spellData.damage), cardController, cardMovement.cardController);
            ActionSystem.instance.Perform(dealDamageGa);

            if (isLastTarget)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                DealDamageGA secondAttack = new DealDamageGA(ComputeCurrentDamage(spellData.damage), cardController, cardMovement.cardController);
                ActionSystem.instance.Perform(secondAttack);
                
            }
            
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            if (!isLastTarget)
            {
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.RacoonLastTarget, 1, cardController, cardMovement.cardController);
                ActionSystem.instance.Perform(applyStatusGa);
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            }
        }

        private IEnumerator ClearStacks(CardMovement cardMovement)
        {
            if (cardMovement.cardController.cardStatus.IsStatusApplied(StatusType.RacoonLastTarget))
            {
                ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.RacoonLastTarget, 1, cardController, cardMovement.cardController);
                ActionSystem.instance.Perform(consumeStacksGa);
                
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            }
        }
    }
}
