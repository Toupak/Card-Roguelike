using System.Collections;
using System.Collections.Generic;
using Board.Script;
using Cards.Scripts;
using Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatLoop
{
    public class PlayerHandController : MonoBehaviour
    {
        [SerializeField] private CardContainer handContainer;
        [SerializeField] private CardMovement cardMovementPrefab;
        [SerializeField] private List<CardData> cardData;
        
        public CardContainer container => handContainer;
        
        public IEnumerator DrawHand()
        {
            yield return new WaitForSeconds(0.1f);
            DrawCard();
            yield return new WaitForSeconds(0.1f);
            DrawCard();
            yield return new WaitForSeconds(0.1f);
            DrawCard();
            yield return new WaitForSeconds(0.1f);
            DrawCard();
            yield return new WaitForSeconds(0.1f);
            DrawCard();
            yield return new WaitForSeconds(0.1f);
            DrawCard();
            yield return new WaitForSeconds(0.1f);
            DrawCard();
            yield return new WaitForSeconds(0.1f);
            DrawCard();
            yield return new WaitForSeconds(0.1f);
        }

        private void Update()
        {
            CheckDrawCard();
        }
        
        private void CheckDrawCard()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame && !handContainer.IsFull)
                DrawCard();
        }

        private void DrawCard()
        {
            CardMovement newCard = Instantiate(cardMovementPrefab);
            handContainer.ReceiveCard(newCard);

            int randomIndex = Random.Range(0, cardData.Count);
            CardController controller = CardsVisualManager.instance.SpawnNewCardVisuals(newCard, cardData[randomIndex]);
            newCard.SetCardController(controller);
        }
    }
}
