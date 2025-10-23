using UnityEngine;

public class SlotContainer : MonoBehaviour
{
    private CardController cardController;
    public bool IsEmpty => cardController == null;


    private void OnEnable()
    {
        //OnCardClicked.AddListener(Highlight);
    }

    private void OnDisable()
    {
        //OnCardClicked.RemoveListener(Highlight);
    }

    private void OnTryPut()
    {

    }

    public void AddCard(CardController card)
    {
        cardController = card;
        card.transform.position = transform.position;
    }

    public void RemoveCard()
    {
        cardController = null;
    }

    private void Highlight()
    {
        //Hover lumineux
    }

    //Slot ID pour que l'adversaire vise la carte située dans le slot 
}
