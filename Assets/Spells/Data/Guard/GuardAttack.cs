using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Spells.Data.Guard
{
    public class GuardAttack : SpellController
    {
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            foreach (CardMovement target in targets)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                DealDamageGA dealDamageGa = new DealDamageGA(ComputeCurrentDamage(spellData.damage), cardController, target.cardController);
                ActionSystem.instance.Perform(dealDamageGa);

                if (target.cardController.cardStatus.IsStatusApplied(StatusType.Vengeance))
                {
                    yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                    int stackCount = target.cardController.cardStatus.GetCurrentStackCount(StatusType.Vengeance);
                    ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.Vengeance, stackCount, cardController, target.cardController);
                    ActionSystem.instance.Perform(consumeStacksGa);
                    
                    yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                    ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Armor, stackCount, cardController, cardController);
                    ActionSystem.instance.Perform(applyStatusGa);
                }
            }
        }
    }
}
