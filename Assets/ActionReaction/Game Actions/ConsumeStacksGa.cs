using Cards.Scripts;

namespace ActionReaction.Game_Actions
{
    public class ConsumeStacksGa : GameAction
    {
        public readonly StatusType type;
        public int amount;
        public readonly CardController attacker;
        public readonly CardController target;

        public bool wasConsumed;

        public ConsumeStacksGa(StatusType statusType, int stacksToConsumeAmount, CardController attacker, CardController target)
        {
            type = statusType;
            amount = stacksToConsumeAmount;
            this.attacker = attacker;
            this.target = target;
        }
    }
}
