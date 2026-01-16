using System.Collections.Generic;
using System.Linq;
using Cards.Scripts;
using Frames;
using UnityEngine;

namespace Run_Loop
{
    public class DeckCard
    {
        public CardData cardData;
        public int currentHealth;
        public FrameData frameData;

        public DeckCard(CardData data)
        {
            cardData = data;
            currentHealth = data.hpMax;
        }
    }
    
    public class PlayerDeck : MonoBehaviour
    {
        public static PlayerDeck instance;

        public List<DeckCard> deck { get; private set; } = new List<DeckCard>();
        public List<DeckCard> lastHandPlayed { get; private set; } = new List<DeckCard>();

        public bool IsEmpty => deck.Count < 1;
        
        private void Awake()
        {
            instance = this;
        }

        public void AddCardToDeck(CardData cardData)
        {
            deck.Add(new DeckCard(cardData));
        }

        public void UpdateCardHealthPoints(DeckCard deckCard, int currentHealth)
        {
            if (deck.Contains(deckCard))
                deck.Find((d) => d == deckCard).currentHealth = currentHealth;
        }

        public void RemoveCardFromDeck(DeckCard deckCard)
        {
            if (deck.Contains(deckCard))
                deck.Remove(deckCard);
        }

        public void ClearDeck()
        {
            deck = new List<DeckCard>();
        }
        
        public bool ContainsCard(CardData cardData)
        {
            return deck.Count((dc) => dc.cardData.name == cardData.name) > 0;
        }

        public void SaveLastHandPlayed(List<DeckCard> cardsToSave)
        {
            lastHandPlayed = cardsToSave.Where((dc) => !dc.cardData.isEnemy && !dc.cardData.isSpecialSummon).ToList();
        }
    }
}
