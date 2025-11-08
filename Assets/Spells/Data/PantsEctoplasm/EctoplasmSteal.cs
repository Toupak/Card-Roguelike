using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Spells.Data.PantsEctoplasm
{
    public class EctoplasmSteal : SpellController
    {
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            foreach (CardMovement target in targets)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(spellData.inflictStatus, spellData.statusStacksApplied, cardController, target.cardController);
                ActionSystem.instance.Perform(applyStatusGa);
                
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                ApplyStatusGa applyPantStack = new ApplyStatusGa(StatusType.Pants, 1, cardController, cardController);
                ActionSystem.instance.Perform(applyPantStack);
            }
        }
    }
}
