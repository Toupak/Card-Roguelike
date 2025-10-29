using Cards.Scripts;

namespace ActionReaction.Game_Actions
{
    public class DealDamageGA : GameAction
    {
        public readonly int amount;
        public readonly CardController target;

        public DealDamageGA(int damageAmount, CardController targetController)
        {
            amount = damageAmount;
            target = targetController;
        }
    }
}
