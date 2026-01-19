using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Spells.Data.Trongle
{
    public class TrongleFG : SpellController
    {
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            foreach (CardMovement target in targets)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                
                if (target.cardController.cardStatus.IsStatusApplied(spellData.inflictStatus))
                {
                    ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(spellData.inflictStatus, spellData.statusStacksApplied, cardController, target.cardController);
                    ActionSystem.instance.Perform(consumeStacksGa);
                }
                else
                {
                    ApplyStatusGa applyStatusGa = new ApplyStatusGa(spellData.inflictStatus, spellData.statusStacksApplied, cardController, target.cardController);
                    ActionSystem.instance.Perform(applyStatusGa);
                }
            }
        }
    }
}
