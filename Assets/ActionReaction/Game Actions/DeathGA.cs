using Cards.Scripts;

namespace ActionReaction.Game_Actions
{
    public class DeathGA : GameAction
    {
        public CardController killer;
        public CardController target;
        public bool isEnemy;

        public bool isDeathPrevented;

        public DeathGA(CardController killerController, CardController targetController)
        {
            killer = killerController;
            target = targetController;
            isEnemy = target.cardMovement.IsEnemyCard;
        }

        public void PreventDeath()
        {
            isDeathPrevented = true;
        }
    }
}
