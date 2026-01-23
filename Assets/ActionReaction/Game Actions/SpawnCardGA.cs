using Cards.Scripts;
using Inventory.Items.Frames;
using Run_Loop;

namespace ActionReaction.Game_Actions
{
    public class SpawnCardGA : GameAction
    {
        public CardData cardData;
        public CardController spawner;
        public CardController spawnedCard;

        public bool isToken;
        public int startingHealth = -1;
        public FrameData frameData = null;
        public DeckCard deckCard = null;

        public SpawnCardGA(CardData data, CardController spawnerController, bool isCardAToken = false)
        {
            cardData = data;
            spawner = spawnerController;
            isToken = isCardAToken;
        }
    }
}
