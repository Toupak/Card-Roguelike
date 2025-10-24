using System.Collections.Generic;
using Cards;
using Cards.Scripts;
using UnityEngine;

namespace CombatLoop
{
    public class Deck : MonoBehaviour
    {
        public static Deck instance;

        [SerializeField] CardMovement cardPrefab;

        [SerializeField] private List<CardData> cardsData;

        private void Awake()
        {
            instance = this;
        }

        public List<CardMovement> DrawCards()
        {
            List<CardMovement> cards = new List<CardMovement>();

            Vector3 position = transform.position;
        
            foreach(CardData cardData in cardsData)
            {
                CardMovement cardTemp = Instantiate(cardPrefab, position, Quaternion.identity);

                cards.Add(cardTemp);
            }

            return cards;
        }

        public void AddCard(CardData card)
        {
            cardsData.Add(card);
        }

        public void RemoveCard(CardData card)
        {
            if (cardsData.Contains(card))
                cardsData.Remove(card);
        }
    }
}
