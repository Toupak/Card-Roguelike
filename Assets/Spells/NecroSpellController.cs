using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Spells.Targeting;
using UnityEngine;

namespace Spells
{
    public class NecroSpellController : SpellController
    {
        [Space]
        [SerializeField] protected int corpseCost;

        public override bool CanCastSpell()
        {
            return base.CanCastSpell() && ComputeCorpseCount() >= corpseCost;
        }
        
        protected IEnumerator ConsumeCorpses(int count)
        {
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            
            int stacks = Mathf.Min(count, cardController.cardStatus.GetCurrentStackCount(StatusType.Corpse));
            if (stacks > 0)
            {
                ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.Corpse, stacks, cardController, cardController);
                ActionSystem.instance.Perform(consumeStacksGa);
            }

            int remaining = count - stacks;
            if (remaining == 0)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                yield break;
            }

            List<CardMovement> targets = ComputeCorpseList();
            for (int i = 0; i < targets.Count && remaining > 0; i++)
            {
                int otherCardsStacks = Mathf.Min(remaining, targets[i].cardController.cardStatus.GetCurrentStackCount(StatusType.Corpse));
                if (otherCardsStacks > 0)
                {
                    yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                    ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.Corpse, otherCardsStacks, cardController, targets[i].cardController);
                    ActionSystem.instance.Perform(consumeStacksGa);
                }

                remaining -= otherCardsStacks;
            }
            
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
        }

        private List<CardMovement> ComputeCorpseList()
        {
            return TargetingSystem.instance.RetrieveBoard(TargetType.Ally).Where((c) => c.cardController.cardStatus.IsStatusApplied(StatusType.Corpse)).ToList();
        }

        protected int ComputeCorpseCount()
        {
            List<CardMovement> holders = ComputeCorpseList();

            int total = 0;
            foreach (CardMovement movement in holders)
            {
                total += movement.cardController.cardStatus.GetCurrentStackCount(StatusType.Corpse);
            }

            return total;
        }
    }
}
