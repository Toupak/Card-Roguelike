using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Spells.Data.Nachito
{
    public class DoritoCaltrops : SpellController
    {
        [SerializeField] private int stacksAppliedCount;
        
        protected override IEnumerator CastSpellOnTarget(SpellData spellData, List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(spellData, targets);

            foreach (CardMovement cardMovement in targets)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                
                Debug.Log($"Target : {cardMovement.cardController.cardData.cardName} / {spellData.targetType}");
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.DoritoCaltrop, stacksAppliedCount, cardController, cardMovement.cardController);
                ActionSystem.instance.Perform(applyStatusGa);
            }
        }
    }
}
