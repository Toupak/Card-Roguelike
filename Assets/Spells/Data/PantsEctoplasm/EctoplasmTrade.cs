using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Spells.Targeting;
using UnityEngine;

namespace Spells.Data.PantsEctoplasm
{
    public class EctoplasmTrade : SpellController
    {
        private int ComputePantStackAmount()
        {
            if (!cardController.cardStatus.IsStatusApplied(StatusType.Pants))
                return 0;

            return cardController.cardStatus.currentStacks[StatusType.Pants];
        }
        
        private int ComputeCost(int pantStacks)
        {
            if (pantStacks >= 5)
                return 5;
            else if (pantStacks >= 3)
                return 3;
            else if (pantStacks >= 1)
                return 1;
            return 0;
        }
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            int cost = ComputeCost(ComputePantStackAmount());

            foreach (CardMovement target in targets)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                yield return ComputeTrade(cost, target.cardController);
            }
            
            yield return ConsumePantStack(cost);
        }

        private IEnumerator ComputeTrade(int cost, CardController target)
        {
            if (cost == 5)
                yield return BigBuff(target);
            else if (cost == 3)
                yield return MediumBuff(target);
            else if (cost == 1)
                yield return SmallDebuff(target);
        }

        private IEnumerator BigBuff(CardController target)
        {
            ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Fury, 1, cardController, target);
            ActionSystem.instance.Perform(applyStatusGa);
            yield break;
        }

        private IEnumerator MediumBuff(CardController target)
        {
            ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.BonusDamage, 2, cardController, target);
            ActionSystem.instance.Perform(applyStatusGa);
            yield break;
        }

        private IEnumerator SmallDebuff(CardController target)
        {
            ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Weak, 1, cardController, target);
            ActionSystem.instance.Perform(applyStatusGa);
            yield break;
        }

        private IEnumerator ConsumePantStack(int cost)
        {
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.Pants, cost, cardController, cardController);
            ActionSystem.instance.Perform(consumeStacksGa);
        }
    }
}
