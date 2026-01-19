using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Status;
using UnityEngine;

namespace Combat.Spells.Data.Nachito
{
    public class SourCreamSpell : SpellController
    {
        public override bool CanCastSpell()
        {
            if (!base.CanCastSpell())
                return false;
            
            List<CardMovement> targets = StatusSystem.instance.GetListOfCardsAfflictedByStatus(TargetType.Enemy, StatusType.DoritoCaltrop);

            return targets != null && targets.Count > 0;
        }
        
        protected override IEnumerator CastSpellOnTarget( List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

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

                DealDamageGA dealDamageGa = new DealDamageGA(ComputeCurrentDamage(stackCount), cardController, target.cardController);
                ActionSystem.instance.Perform(dealDamageGa);
            }
        }
    }
}
