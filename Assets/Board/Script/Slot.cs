using UnityEngine;

public class Slot : MonoBehaviour
{
    private CardController cardController;

    public bool IsEmpty => cardController == null;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ReceiveCard(CardController card)
    {
        cardController = card;
    }

    public CardController RemoveCard()
    {
        CardController cardTemp = cardController;
        cardController = null;

        return cardTemp;
    }
}
