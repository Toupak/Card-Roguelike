using Cards.Scripts;

namespace ActionReaction.Game_Actions
{
    public class SpawnCardGA : GameAction
    {
        public CardData cardData;
        public CardController spawner;

        public SpawnCardGA(CardData data, CardController spawnerController)
        {
            cardData = data;
            spawner = spawnerController;
        }
    }
}
