using Cards.Scripts;

namespace ActionReaction.Game_Actions
{
    public class DealDamageGA : GameAction
    {
        public int amount;
        public readonly CardController attacker;
        public CardController target;

        public bool isDamageNegated;

        public bool isBongoAttack;

        public DealDamageGA(int damageAmount, CardController attackerController, CardController targetController)
        {
            amount = damageAmount;
            attacker = attackerController;
            target = targetController;
        }

        public void NegateDamage()
        {
            amount = 0;
            isDamageNegated = true;
        }
    }
}
