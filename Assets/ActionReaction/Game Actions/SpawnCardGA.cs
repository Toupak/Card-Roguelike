using Cards.Scripts;

namespace ActionReaction.Game_Actions
{
    public class SpawnCardGA : GameAction
    {
        public CardData cardData;
        public CardController spawner;
        public CardController spawnedCard;

        public bool isToken;

        public SpawnCardGA(CardData data, CardController spawnerController, bool isCardAToken = false)
        {
            cardData = data;
            spawner = spawnerController;
            isToken = isCardAToken;
        }
    }
}
