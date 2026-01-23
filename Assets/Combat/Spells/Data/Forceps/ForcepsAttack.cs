using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.Spells.Data.Forceps
{
    public class ForcepsAttack : SpellController
    {
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            
            SetShinyState(false);

            foreach (CardMovement target in targets)
            {
                DealDamageGA dealDamageGa = new DealDamageGA(ComputeCurrentDamage(spellData.damage), cardController, target.cardController);
                ActionSystem.instance.Perform(dealDamageGa);
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                if (target.cardController.cardStatus.IsStatusApplied(StatusType.Vengeance))
                {
                    ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.Vengeance, 1, cardController, target.cardController);
                    ActionSystem.instance.Perform(consumeStacksGa);
                    yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                    
                    SetShinyState(true);

                    ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Vengeance, 1, cardController, cardController);
                    ActionSystem.instance.Perform(applyStatusGa);
                    yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                }
            }
        }
    }
}
