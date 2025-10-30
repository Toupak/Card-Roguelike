using BoomLib.Tools;
using static CombatLoop.CombatLoop;

namespace ActionReaction.Game_Actions
{
    public class EndTurnGA : GameAction
    {
        public TurnType ending;
        public TurnType starting;

        public EndTurnGA(TurnType current)
        {
            ending = current;
            starting = current.Opposite();
        }
    }
}
