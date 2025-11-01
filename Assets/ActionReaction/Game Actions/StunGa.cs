using Cards.Scripts;

namespace ActionReaction.Game_Actions
{
    public class StunGa : GameAction
    {
        public readonly int amount;
        public readonly CardController attacker;
        public readonly CardController target;

        public StunGa(int stunAmount, CardController attackerController, CardController targetController)
        {
            amount = stunAmount;
            attacker = attackerController;
            target = targetController;
        }
    }
}
