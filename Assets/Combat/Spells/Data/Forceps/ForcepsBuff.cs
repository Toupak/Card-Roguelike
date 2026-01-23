using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.Spells.Data.Forceps
{
    public class ForcepsBuff : SpellController
    {
        [SerializeField] private int vengeanceCost;
        
        public override bool CanCastSpell()
        {
            return base.CanCastSpell() && cardController.cardStatus.GetCurrentStackCount(StatusType.Vengeance) >= vengeanceCost;
        }

        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.Vengeance, cardController.cardStatus.GetCurrentStackCount(StatusType.Vengeance), cardController, cardController);
            ActionSystem.instance.Perform(consumeStacksGa);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            ApplyStatusGa applyStatusGa = new ApplyStatusGa(spellData.inflictStatus, spellData.statusStacksApplied, cardController, cardController);
            ActionSystem.instance.Perform(applyStatusGa);
        }
    }
}
