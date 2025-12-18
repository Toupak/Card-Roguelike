using System;
using System.Collections.Generic;
using Board.Script;
using Cards.Scripts;
using Overworld.Character;
using Run_Loop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Character_Selection
{
    public class CharacterSelectionLoop : MonoBehaviour
    {
        [SerializeField] private Image characterSprite;
        [SerializeField] private TextMeshProUGUI characterName;
        [SerializeField] private TextMeshProUGUI characterDescription;
        [SerializeField] private CardContainer cardContainer;
        
        [Space]
        [SerializeField] private CardMovement cardMovementPrefab;

        [Space]
        [SerializeField] private List<OverWorldCharacterData> characterData;

        public static CharacterSelectionLoop instance;

        [HideInInspector] public bool isCharacterSelected;
        
        private int currentCharacterIndex;
        
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            LoadCharacter(0);
        }

        private void LoadCharacter(int index)
        {
            currentCharacterIndex = index;

            characterSprite.sprite = characterData[index].displaySprite;
            characterName.text = characterData[index].characterName;
            characterDescription.text = characterData[index].description;
            
            DeleteAllCards();
            SpawnPlayerStartingCards(characterData[index].startingCards);
        }

        private void DeleteAllCards()
        {
            for (int i = cardContainer.Slots.Count - 1; i >= 0; i--)
            {
                cardContainer.Slots[i].CurrentCard.cardController.KillCard(false);
            }
        }
        
        private void SpawnPlayerStartingCards(List<CardData> startingCards)
        {
            foreach (CardData card in startingCards)
            {
                SpawnCard(new DeckCard(card));
            }
        }

        private void SpawnCard(DeckCard deckCard)
        {
            CardMovement newCard = Instantiate(cardMovementPrefab);
            cardContainer.ReceiveCard(newCard);

            CardController controller = CardsVisualManager.instance.SpawnNewCardVisuals(newCard, deckCard);
            newCard.SetCardController(controller);
        }

        public void ShowNextCharacter()
        {
            if (currentCharacterIndex + 1 < characterData.Count)
                LoadCharacter(currentCharacterIndex + 1);
        }

        public void ShowPreviousCharacter()
        {
            if (currentCharacterIndex - 1 >= 0)
                LoadCharacter(currentCharacterIndex - 1);
        }
        
        public void ValidateSelection()
        {
            isCharacterSelected = true;
        }

        public OverWorldCharacterData GetSelectedCharacter()
        {
            return characterData[currentCharacterIndex];
        }
    }
}
