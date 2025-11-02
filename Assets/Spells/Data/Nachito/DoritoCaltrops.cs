using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Spells.Data.Nachito
{
    public class DoritoCaltrops : SpellController
    {
        [SerializeField] private int stacksAppliedCount;
        [SerializeField] private int damageOnPerformAction;
        
        public Dictionary<CardController, int> stacksDictionary { get; private set; } = new Dictionary<CardController, int>();
        public bool HasTargets => stacksDictionary.Count > 0;
        
        protected override IEnumerator CastSpellOnTarget(SpellData spellData, List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(spellData, targets);

            foreach (CardMovement cardMovement in targets)
            {
                AddTargetToDictionary(cardMovement.cardController, stacksAppliedCount);
            }
        }

        private void AddTargetToDictionary(CardController target, int stacks)
        {
            if (stacksDictionary.ContainsKey(target))
                stacksDictionary[target] += stacks;
            else
                stacksDictionary.Add(target, stacks);
        }

        public void ClearAllStacks()
        {
            stacksDictionary = new Dictionary<CardController, int>();
        }

        protected override void SubscribeReactions()
        {
            base.SubscribeReactions();
            ActionSystem.SubscribeReaction<EnemyPerformsActionGa>(EnemyPerformsActionReaction, ReactionTiming.POST);
            ActionSystem.SubscribeReaction<EndTurnGA>(EndTurnReaction, ReactionTiming.PRE);
        }

        protected override void UnsubscribeReactions()
        {
            base.UnsubscribeReactions();
            ActionSystem.UnsubscribeReaction<EnemyPerformsActionGa>(EnemyPerformsActionReaction, ReactionTiming.POST);
            ActionSystem.UnsubscribeReaction<EndTurnGA>(EndTurnReaction, ReactionTiming.PRE);
        }
        
        public void EnemyPerformsActionReaction(EnemyPerformsActionGa enemyPerformsActionGa)
        {
            if (stacksDictionary.ContainsKey(enemyPerformsActionGa.cardController))
                enemyPerformsActionGa.cardController.cardHealth.TakeDamage(damageOnPerformAction);
        }
        
        public void EndTurnReaction(EndTurnGA endTurnGa)
        {
            if (endTurnGa.ending == CombatLoop.CombatLoop.TurnType.Enemy)
            {
                stacksDictionary = stacksDictionary.Where(pair => pair.Value > 1)
                    .ToDictionary(pair => pair.Key, pair => pair.Value - 1);
            }
        }
    }
}
