using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public static Deck instance;

    [SerializeField] CardController cardPrefab;

    [SerializeField] private List<CardData> cardsData;

    private void Awake()
    {
        instance = this;
    }

    public List<CardController> DrawCards()
    {
        List<CardController> cards = new List<CardController>();

        foreach(CardData cardData in cardsData)
        {
            CardController cardTemp = Instantiate(cardPrefab);
            cardTemp.Setup(cardData);

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
