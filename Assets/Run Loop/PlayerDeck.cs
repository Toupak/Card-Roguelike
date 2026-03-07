using System.Collections.Generic;
using System.Linq;
using BoomLib.Tools;
using Cards.Scripts;
using UnityEngine;

namespace Run_Loop
{
    public class PlayerDeck : MonoBehaviour
    {
        public static PlayerDeck instance;

        public List<CardData> deck { get; private set; } = new List<CardData>();
        public List<CardData> lastHandPlayed { get; private set; } = new List<CardData>();

        public bool IsEmpty => deck.Count < 1;
        
        private void Awake()
        {
            instance = this;
        }

        public void AddCardToDeck(CardData cardData)
        {
            CardData deckCard = cardData.Clone();
            deckCard.currentHp = cardData.hpMax;
            
            deck.Add(deckCard);
        }

        public void UpdateCardHealthPoints(CardData card, int currentHealth)
        {
            if (deck.Contains(card))
                deck.Find((d) => d == card).currentHp = currentHealth;
        }

        public void RemoveCardFromDeck(CardData deckCard)
        {
            if (deck.Contains(deckCard))
                deck.Remove(deckCard);
            if (lastHandPlayed.Contains(deckCard))
                lastHandPlayed.Remove(deckCard);
        }

        public void ClearDeck()
        {
            deck.Clear();
        }
        
        public bool ContainsCard(CardData cardData)
        {
            return deck.Contains(cardData);
        }

        public void SaveLastHandPlayed(List<CardData> cardsToSave)
        {
            lastHandPlayed = cardsToSave.Where((dc) => !dc.isEnemy && !dc.isSpecialSummon).ToList();
        }
    }
}
