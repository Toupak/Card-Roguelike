using Cards.Scripts;

namespace ActionReaction.Game_Actions
{
    public class HealGa : GameAction
    {
        public int amount;
        public readonly CardController attacker;
        public readonly CardController target;

        public HealGa(int healAmount, CardController attackerController, CardController targetController)
        {
            amount = healAmount;
            attacker = attackerController;
            target = targetController;
        }

        public void NegateHeal()
        {
            amount = 0;
        }
    }
}
