using Cards.Scripts;

namespace ActionReaction.Game_Actions
{
    public class StunGa : GameAction
    {
        public readonly int amount;
        public readonly CardController target;

        public StunGa(int damageAmount, CardController targetController)
        {
            amount = damageAmount;
            target = targetController;
        }
    }
}
