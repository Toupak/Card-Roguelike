using Cards.Scripts;

namespace Combat.Status.Data.Terror
{
    public class TerrorController : StatusController
    {
        public override void AddStack(int amount)
        {
            if (cardController.cardStatus.GetCurrentStackCount(StatusType.Terror) == amount)
                cardController.cardStats.DecreaseStat(CardStats.Stats.Strength, 1);
        }

        public override void RemoveStack(int amount)
        {
            if (cardController.cardStatus.GetCurrentStackCount(StatusType.Terror) == 0)
                cardController.cardStats.IncreaseStat(CardStats.Stats.Strength, 1);
        }
    }
}
