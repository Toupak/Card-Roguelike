using static Combat.CombatLoop;

namespace ActionReaction.Game_Actions
{
    public class EndTurnGA : GameAction
    {
        public TurnType ending;

        public EndTurnGA(TurnType current)
        {
            ending = current;
        }
    }
}
