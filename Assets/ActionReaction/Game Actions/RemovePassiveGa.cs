using Cards.Scripts;
using Passives;

namespace ActionReaction.Game_Actions
{
    public class RemovePassiveGa : GameAction
    {
        public CardController caster;
        public CardController target;
        public PassiveData passive;

        public RemovePassiveGa(CardController casterController, CardController targetController, PassiveData passiveData)
        {
            caster = casterController;
            target = targetController;
            passive = passiveData;
        }
    }
}
