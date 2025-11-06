using System.Collections.Generic;
using Cards.Scripts;
using UnityEngine;

namespace Run_Loop
{
    public class DeckCard
    {
        public CardData cardData;
        public int currentHealth;

        public DeckCard(CardData data, int maxHealth)
        {
            cardData = data;
            currentHealth = maxHealth;
        }
    }
    
    public class PlayerDeck : MonoBehaviour
    {
        public static PlayerDeck instance;

        private List<DeckCard> deck = new List<DeckCard>();
        
        private void Awake()
        {
            instance = this;
        }

        public void AddCardToDeck(CardData cardData)
        {
            deck.Add(new DeckCard(cardData, cardData.hpMax));
        }
    }
}
