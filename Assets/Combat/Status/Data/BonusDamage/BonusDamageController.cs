using Cards.Scripts;

namespace Combat.Status.Data.BonusDamage
{
    public class BonusDamageController : StatusController
    {
        public override void AddStack(int amount)
        {
            cardController.cardStats.IncreaseStat(CardStats.Stats.Strength, amount);
        }

        public override void RemoveStack(int amount)
        {
            cardController.cardStats.DecreaseStat(CardStats.Stats.Strength, amount);
        }
    }
}
