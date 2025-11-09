using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Spells.Data.Dino_And_Zaur
{
    public class DinoAndZaurAttack : SpellController
    {
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            int damage = ComputeCurrentDamage(spellData.damage);

            foreach (CardMovement target in targets)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                DealDamageGA dealDamageGa = new DealDamageGA(damage, cardController, target.cardController);
                ActionSystem.instance.Perform(dealDamageGa);

                damage -= 1;

                if (damage <= 0)
                    break;
            }
        }
    }
}
