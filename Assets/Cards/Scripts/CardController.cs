using System;
using TMPro;
using UnityEngine;

public class CardController : MonoBehaviour
{
    [SerializeField] private TextMeshPro cardName;
    [SerializeField] private TextMeshPro cardNumber;

    public void Setup(CardData cardData)
    {
        cardName.text = cardData.cardName;
        cardNumber.text = cardData.cardNumber.ToString();
    }
}
