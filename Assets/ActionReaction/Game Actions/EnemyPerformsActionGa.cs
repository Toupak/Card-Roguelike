using Cards.Scripts;

namespace ActionReaction.Game_Actions
{
    public class EnemyPerformsActionGa : GameAction
    {
        public CardController cardController;

        public EnemyPerformsActionGa(CardController cardController)
        {
            this.cardController = cardController;
        }
    }
}
