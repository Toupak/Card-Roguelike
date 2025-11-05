using Cards.Scripts;

namespace ActionReaction.Game_Actions
{
    public class RefreshCooldownGA : GameAction
    {
        public CardController caster;
        public CardController target;

        public RefreshCooldownGA(CardController casterController, CardController targetController)
        {
            caster = casterController;
            target = targetController;
        }
    }
}
