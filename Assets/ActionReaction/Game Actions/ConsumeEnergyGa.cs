using Combat.Spells;

namespace ActionReaction.Game_Actions
{
    public class ConsumeEnergyGa : GameAction
    {
        public int cost;
        public SpellController caster;

        public ConsumeEnergyGa(int energyCost, SpellController spellController)
        {
            cost = energyCost;
            caster = spellController;
        }
    }
}
