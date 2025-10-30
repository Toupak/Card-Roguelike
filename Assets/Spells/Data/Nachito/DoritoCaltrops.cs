using System.Collections;
using System.Collections.Generic;
using Cards.Scripts;
using UnityEngine;

namespace Spells.Data.Nachito
{
    public class DoritoCaltrops : SpellController
    {
        [SerializeField] private int stacksAppliedCount;
        
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

        private void OnEnable()
        {
            //ActionSystem.SubscribeReaction<CardPerformActionGA>(CardPerformActionReaction, ReactionTiming.POST);
        }

        private void OnDisable()
        {
            //ActionSystem.UnsubscribeReaction<CardPerformActionGA>(CardPerformActionReaction, ReactionTiming.POST);
        }
        
        /*
        public void StartTurnReaction(CardPerformActionGA cardPerformActionGA)
        {
            //cardPerformActionGA.controller.cardHealth.TakeDamage();
        }
        */
    }
}
