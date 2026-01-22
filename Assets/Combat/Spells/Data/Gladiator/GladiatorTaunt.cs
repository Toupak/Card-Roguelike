using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.Spells.Data.Gladiator
{
    public class GladiatorTaunt : SpellController
    {
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            ApplyStatusGa applyStatusGa = new ApplyStatusGa(spellData.inflictStatus, spellData.statusStacksApplied, cardController, cardController);
            ActionSystem.instance.Perform(applyStatusGa);
            
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            foreach (CardMovement cardMovement in targets)
            {
                if (cardMovement.cardController.cardStatus.IsStatusApplied(StatusType.Spear))
                {
                    int spearCount = cardMovement.cardController.cardStatus.GetCurrentStackCount(StatusType.Spear);
                    
                    ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.Spear, spearCount, cardController, cardMovement.cardController);
                    ActionSystem.instance.Perform(consumeStacksGa);
                    yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                    ApplyStatusGa applyArmor = new ApplyStatusGa(StatusType.Armor, spearCount, cardController, cardController);
                    ActionSystem.instance.Perform(applyArmor);
                    yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                }
            }
        }
    }
}
