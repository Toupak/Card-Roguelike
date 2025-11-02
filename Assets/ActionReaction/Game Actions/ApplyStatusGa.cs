using Cards.Scripts;

namespace ActionReaction.Game_Actions
{
    public class ApplyStatusGa : GameAction
    {
        public readonly StatusType type;
        public readonly int amount;
        public readonly CardController attacker;
        public readonly CardController target;

        public ApplyStatusGa(StatusType statusType, int stacksAmount, CardController attackerController, CardController targetController)
        {
            type = statusType;
            amount = stacksAmount;
            attacker = attackerController;
            target = targetController;
        }
    }
}
