using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Spells.Data.Barbarian
{
    public class BarbarianDefense : SpellController
    {
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Taunt, spellData.statusStacksApplied, cardController, cardController);
            ActionSystem.instance.Perform(applyStatusGa);

            if (cardController.cardStatus.IsStatusApplied(StatusType.BerserkMode))
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                ApplyStatusGa returnDamage = new ApplyStatusGa(StatusType.ReturnDamage, spellData.statusStacksApplied, cardController, cardController);
                ActionSystem.instance.Perform(returnDamage);
            }
        }
    }
}
