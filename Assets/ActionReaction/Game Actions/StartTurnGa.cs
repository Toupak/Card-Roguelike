using static Combat.CombatLoop;

namespace ActionReaction.Game_Actions
{
    public class StartTurnGa : GameAction
    {
        public TurnType starting;
        public int turnCount;

        public StartTurnGa(TurnType current, int turnCount)
        {
            starting = current;
            this.turnCount = turnCount;
        }
    }
}
