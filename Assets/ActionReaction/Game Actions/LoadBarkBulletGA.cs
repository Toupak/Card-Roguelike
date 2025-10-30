using Cards.Scripts;

namespace ActionReaction.Game_Actions
{
    public class LoadBarkBulletGA : GameAction
    {
        public readonly CardController target;

        public LoadBarkBulletGA(CardController targetController)
        {
            target = targetController;
        }
    }
}
