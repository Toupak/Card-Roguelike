using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using CombatLoop;
using Status;
using UnityEngine;

namespace Spells.Data.Nachito
{
    public class SourCreamSpell : SpellController
    {
        public override bool CanCastSpell(SpellData spellData)
        {
            List<CardMovement> targets = StatusSystem.instance.GetListOfCardsAfflictedByStatus(TargetType.Enemy, StatusType.DoritoCaltrop);

            return base.CanCastSpell(spellData) && targets != null && targets.Count > 0;
        }
        
        protected override IEnumerator CastSpellOnTarget(SpellData spellData, List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(spellData, targets);

            foreach (CardMovement target in targets)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                
                Debug.Log($"Target : {target.cardController.cardData.cardName} / {spellData.targetType}");
                
                if (!target.cardController.cardStatus.currentStacks.ContainsKey(StatusType.DoritoCaltrop))
                    continue;

                int stackCount = target.cardController.cardStatus.currentStacks[StatusType.DoritoCaltrop];
                
                if (stackCount < 1)
                    continue;
                
                ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.DoritoCaltrop, stackCount, cardController, target.cardController);
                ActionSystem.instance.Perform(consumeStacksGa);
                
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                DealDamageGA dealDamageGa = new DealDamageGA(stackCount, cardController, target.cardController);
                ActionSystem.instance.Perform(dealDamageGa);
            }
        }
    }
}
