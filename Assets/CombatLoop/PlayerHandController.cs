using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandController : MonoBehaviour
{
    private List<CardController> cards;

    public IEnumerator DrawHand()
    {
        cards = Deck.instance.DrawCards();

        for (int i = 0; i < cards.Count; i++)
        {
            yield return PlaceCardInHand(cards[i], i);
        }
    }

    private IEnumerator PlaceCardInHand(CardController card, int i)
    {
        yield return null;
    }
}
