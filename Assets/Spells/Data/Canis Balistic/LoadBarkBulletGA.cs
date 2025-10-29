using ActionReaction;
using Cards.Scripts;

namespace Spells.Data.Canis_Balistic
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
