using Cards.Scripts;
using Spells;

namespace ActionReaction.Game_Actions
{
    public class GainEnergyGa : GameAction
    {
        public int amount;
        public CardController caster;

        public GainEnergyGa(int amountGained, CardController casterController)
        {
            amount = amountGained;
            caster = casterController;
        }
    }
}