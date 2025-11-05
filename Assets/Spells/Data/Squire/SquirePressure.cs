using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Spells.Data.Squire
{
    public class SquirePressure : SpellController
    {
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            
            foreach (CardMovement target in targets)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                
                Debug.Log($"Target : {target.cardController.cardData.cardName} / {spellData.targetType}");
                
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(spellData.inflictStatus, spellData.statusStacksApplied, cardController, target.cardController);
                ActionSystem.instance.Perform(applyStatusGa);
            }

            if (otherSpellButton.spellController.HasCastedThisTurn)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                ApplyStatusGa selfStun = new ApplyStatusGa(StatusType.Stun, 2, cardController, cardController);
                ActionSystem.instance.Perform(selfStun);
            }
        }
    }
}
