using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Spells.Data.Xachary
{
    public class XacharyTransfer : SpellController
    {
        public override bool CanCastSpell()
        {
            return base.CanCastSpell() && cardController.cardStatus.IsStatusApplied(StatusType.BulletAmmo);
        }
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            int stacks = cardController.cardStatus.GetCurrentStackCount(StatusType.BulletAmmo);
            
            if (stacks < 1)
                yield break;

            ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.BulletAmmo, stacks, cardController, cardController);
            ActionSystem.instance.Perform(consumeStacksGa);
            
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.BulletAmmo, stacks, cardController, targets[0].cardController);
            ActionSystem.instance.Perform(applyStatusGa);
        }
    }
}
