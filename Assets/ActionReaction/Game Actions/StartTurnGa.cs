using static CombatLoop.CombatLoop;

namespace ActionReaction.Game_Actions
{
    public class StartTurnGa : GameAction
    {
        public TurnType starting;

        public StartTurnGa(TurnType current)
        {
            starting = current;
        }
    }
}
