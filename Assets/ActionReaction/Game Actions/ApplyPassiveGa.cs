using Cards.Scripts;
using Passives;

namespace ActionReaction.Game_Actions
{
    public class ApplyPassiveGa : GameAction
    {
        public CardController caster;
        public CardController target;
        public PassiveData passive;

        public ApplyPassiveGa(CardController casterController, CardController targetController, PassiveData passiveData)
        {
            caster = casterController;
            target = targetController;
            passive = passiveData;
        }
    }
}
