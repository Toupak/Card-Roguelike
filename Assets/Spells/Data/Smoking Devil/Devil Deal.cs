using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Spells.Data.Smoking_Devil
{
    public class DevilDeal : SpellController
    {
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            foreach (CardMovement target in targets)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                Debug.Log($"Target : {target.cardController.cardData.cardName} / {spellData.targetType}");

                DealDamageGA damageGA = new DealDamageGA(spellData.damage, cardController, target.cardController);
                ActionSystem.instance.Perform(damageGA);

                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                if (target != null)
                {
                    ApplyStatusGa statusGa = new ApplyStatusGa(StatusType.BonusDamage, spellData.statusStacksApplied, cardController, target.cardController);
                    ActionSystem.instance.Perform(statusGa);
                }
            }
        }
    }
}
