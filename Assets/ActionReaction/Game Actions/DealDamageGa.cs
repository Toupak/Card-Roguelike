using Cards.Scripts;
using UnityEngine;

namespace ActionReaction.Game_Actions
{
    public class DealDamageGA : GameAction
    {
        public int amount;
        public readonly CardController attacker;
        public CardController target;

        public bool isDamageNegated;
        
        public bool isTargetSwitched;
        public CardController originalTarget;

        public bool isBongoAttack;

        public DealDamageGA(int damageAmount, CardController attackerController, CardController targetController)
        {
            amount = Mathf.Abs(damageAmount);
            attacker = attackerController;
            target = targetController;
        }

        public void NegateDamage()
        {
            amount = 0;
            isDamageNegated = true;
        }

        public void SwitchTarget(CardController newTarget)
        {
            originalTarget = target;
            target = newTarget;
            isTargetSwitched = true;
        }
    }
}
