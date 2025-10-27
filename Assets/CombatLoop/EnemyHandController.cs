using System.Collections.Generic;
using Board.Script;
using Cards.Scripts;
using Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatLoop
{
    public class EnemyHandController : MonoBehaviour
    {
        [SerializeField] private CardContainer enemyBoardContainer;
        [SerializeField] private CardMovement cardMovementPrefab;
        [SerializeField] private List<CardData> cardData;
        
        public bool IsOver { get; internal set; }

        internal void StartPlayTurn()
        {
        }
        
        private void Update()
        {
            CheckDrawCard();
        }
        
        private void CheckDrawCard()
        {
            if (Keyboard.current.eKey.wasPressedThisFrame && !enemyBoardContainer.IsFull)
                DrawCard();
        }
        
        public void DrawCard()
        {
            CardMovement newCard = Instantiate(cardMovementPrefab);
            enemyBoardContainer.ReceiveCard(newCard);

            int randomIndex = Random.Range(0, cardData.Count);
            CardController controller = CardsVisualManager.instance.SpawnNewCardVisuals(newCard, cardData[randomIndex]);
            newCard.SetCardController(controller);
        }
    }
}
