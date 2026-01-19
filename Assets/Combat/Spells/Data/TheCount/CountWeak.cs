using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Spells.Targeting;
using UnityEngine;

namespace Combat.Spells.Data.TheCount
{
    public class CountWeak : SpellController
    {
        public override bool CanCastSpell()
        {
            if (!base.CanCastSpell())
                return false;

            bool hasBlood = cardController.cardStatus.GetCurrentStackCount(StatusType.Blood) >= 3;

            if (!hasBlood)
                return false;

            bool hasTarget = TargetingSystem.instance.RetrieveBoard(TargetType.None).Count((c) => c.cardController.cardStatus.IsStatusApplied(StatusType.Weak)) > 0;
            
            return hasTarget;
        }
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.Blood, 3, cardController, cardController);
            ActionSystem.instance.Perform(consumeStacksGa);
            
            foreach (CardMovement target in targets)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                
                if (target.cardController.cardStatus.IsStatusApplied(StatusType.Weak))
                {
                    ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Weak, spellData.statusStacksApplied, cardController, target.cardController);
                    ActionSystem.instance.Perform(applyStatusGa);
                }
            }
        }
    }
}
