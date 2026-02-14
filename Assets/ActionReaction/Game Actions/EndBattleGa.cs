namespace ActionReaction.Game_Actions
{
    public class EndBattleGa : GameAction
    {
        public bool isPlayerWin = false;

        public EndBattleGa(bool isPlayerWin)
        {
            this.isPlayerWin = isPlayerWin;
        }
    }
}
