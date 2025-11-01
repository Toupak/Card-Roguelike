using Cards.Scripts;

namespace ActionReaction.Game_Actions
{
    public class DealDamageGA : GameAction
    {
        public readonly int amount;
        public readonly CardController attacker;
        public readonly CardController target;

        public DealDamageGA(int damageAmount, CardController attackerController, CardController targetController)
        {
            amount = damageAmount;
            target = targetController;
            attacker = attackerController;
        }
    }
}
