using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.Spells.Data.Olaf_And_Pif
{
    public class OlafAndPifAttack : SpellController
    {
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            foreach (CardMovement target in targets)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                
                int damage = spellData.damage;
                if (target.cardController.cardStatus.IsStatusApplied(StatusType.Vengeance))
                {
                    damage = spellData.statusStacksApplied;
                    int stacks = target.cardController.cardStatus.GetCurrentStackCount(StatusType.Vengeance);
                    ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.Vengeance, stacks, cardController, target.cardController);
                    ActionSystem.instance.Perform(consumeStacksGa);
                    yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                }

                DealDamageGA dealDamageGa = new DealDamageGA(ComputeCurrentDamage(damage), cardController, target.cardController);
                ActionSystem.instance.Perform(dealDamageGa);
            }
        }
    }
}
