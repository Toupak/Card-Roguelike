using System.Collections.Generic;
using Cards.Scripts;
using UnityEngine;

namespace CombatLoop
{
    public class Deck : MonoBehaviour
    {
        public static Deck instance;

        [SerializeField] private List<CardData> cardsData;

        private void Awake()
        {
            instance = this;
        }

        public List<CardData> DrawCards()
        {
            return cardsData;
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
