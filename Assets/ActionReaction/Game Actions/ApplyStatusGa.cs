using Cards.Scripts;

namespace ActionReaction.Game_Actions
{
    public class ApplyStatusGa : GameAction
    {
        public readonly StatusType type;
        public int amount;
        public readonly CardController attacker;
        public CardController target;

        public bool isBongoStatus;

        public ApplyStatusGa(StatusType statusType, int stacksAmount, CardController attackerController, CardController targetController)
        {
            type = statusType;
            amount = stacksAmount;
            attacker = attackerController;
            target = targetController;
        }

        public void NegateEffect()
        {
            amount = 0;
        }
    }
}
