using Cards.Scripts;

namespace Combat.Status.Data.Weak
{
    public class WeakController : StatusController
    {
        public override void AddStack(int amount)
        {
            cardController.cardStats.DecreaseStat(CardStats.Stats.Strength, amount);
        }

        public override void RemoveStack(int amount)
        {
            cardController.cardStats.IncreaseStat(CardStats.Stats.Strength, amount);
        }
    }
}
