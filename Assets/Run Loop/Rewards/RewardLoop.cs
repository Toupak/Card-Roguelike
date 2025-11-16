using System.Collections;
using System.Collections.Generic;
using Board.Script;
using BoomLib.Tools;
using Cards.Scripts;
using Cards.Tween_Animations;
using CardSlot.Script;
using CombatLoop;
using Run_Loop.Run_Parameters;
using UnityEngine;

namespace Run_Loop.Rewards
{
    public class RewardLoop : MonoBehaviour
    {
        [SerializeField] private CardContainer mainContainer;
        [SerializeField] private CardContainer handContainer;
        [SerializeField] private CardContainer selectedCardsContainer;
        [SerializeField] private CardContainer selectionContainer;
        
        [Space]
        [SerializeField] private GameObject openBoosterButton;
        [SerializeField] private GameObject selectCardButton;
        [SerializeField] private GameObject validateButton;
        
        [Space]
        [SerializeField] private CardMovement cardMovementPrefab;

        [Space] 
        [SerializeField] private List<CardData> testData;

        public static RewardLoop instance;

        private RunParameterData runParameterData;
        
        public bool isRewardScreenOver { get; private set; }
        
        private void Awake()
        {
            instance = this;
        }

        private IEnumerator Start()
        {
            runParameterData = ComputeRunParameterData();
            //FillDeckForTest();

            bool isFirstRun = IsFirstRun();
            
            if (!isFirstRun)
                yield return LoadCurrentDeckInHand();

            int boosterCount = isFirstRun ? runParameterData.startBoosterCount : runParameterData.boosterCount;
            for (int i = 0; i < boosterCount; i++)
            {
                yield return WaitUntilOpenButtonIsClicked();
                yield return OpenBoosterAndDisplayCards(isFirstRun);
                yield return WaitUntilCardHasBeenSelected();
                if (IsSelectedCardAlreadyInDeck())
                    yield return HealDeckCard();
                else
                    yield return StoreSelectedCard();
                yield return RemoveRemainingCards();
            }

            yield return DisplayFinalSelection();
            yield return WaitUntilFinalValidation();
        }

        private void FillDeckForTest()
        {
            foreach (CardData data in testData)
            {
                PlayerDeck.instance.AddCardToDeck(data);
            }
        }

        private RunParameterData ComputeRunParameterData()
        {
            if (RunLoop.instance != null && RunLoop.instance.currentRunParameterData != null)
                return RunLoop.instance.currentRunParameterData;
            else
                return new RunParameterData(4, 5, 3, 5, 3);
        }
        
        private bool IsFirstRun()
        {
            return PlayerDeck.instance.deck.Count < 1;
        }
        
        private IEnumerator LoadCurrentDeckInHand()
        {
            foreach (DeckCard card in PlayerDeck.instance.deck)
            {
                DrawCardToContainer(card, handContainer);
                yield return new WaitForSeconds(0.1f);
            }
        }

        private IEnumerator WaitUntilOpenButtonIsClicked()
        {
            hasClickedOnOpenBooster = false;
            yield return SetOpenBoosterButtonState(true);
            yield return new WaitUntil(() => hasClickedOnOpenBooster);
            yield return SetOpenBoosterButtonState(false);
        }

        private IEnumerator SetOpenBoosterButtonState(bool state)
        {
            openBoosterButton.SetActive(state);
            yield break;
        }

        private bool hasClickedOnOpenBooster;
        public void OnClickOpenBooster()
        {
            hasClickedOnOpenBooster = true;
        }
        
        private IEnumerator OpenBoosterAndDisplayCards(bool isFirstRun)
        {
            List<CardData> cards = RunLoop.instance.dataBase.GetAllCards((c) => c.canBeDrawn);
            if (cards == null)
            {
                Debug.LogError($"[{nameof(RewardLoop)}] error : no cards found in dataBase");
                yield break;
            }

            List<CardData> shuffledList = new List<CardData>(cards);
            shuffledList.Shuffle();

            int cardCount = isFirstRun ? runParameterData.startCardCount : runParameterData.cardCount;
            for (int i = 0; i < cardCount && i < shuffledList.Count; i++)
            {
                DrawCardToContainer(new DeckCard(shuffledList[i]), mainContainer);
                yield return new WaitForSeconds(0.3f);
            }
        }
        
        private void DrawCardToContainer(DeckCard card, CardContainer container)
        {
            CardMovement newCard = Instantiate(cardMovementPrefab);
            container.ReceiveCard(newCard);
            
            CardController controller = CardsVisualManager.instance.SpawnNewCardVisuals(newCard, card);
            newCard.SetCardController(controller);
        }

        private IEnumerator WaitUntilCardHasBeenSelected()
        {
            hasClickedOnSelect = false;
            yield return new WaitUntil(() => selectionContainer.Slots.Count > 0);
            yield return SetSelectCardButtonState(true);
            yield return new WaitUntil(() => hasClickedOnSelect);
            yield return SetSelectCardButtonState(false);
        }
        
        private IEnumerator SetSelectCardButtonState(bool state)
        {
            selectCardButton.SetActive(state);
            yield break;
        }

        private bool hasClickedOnSelect;
        public void OnClickSelectCard()
        {
            if (selectionContainer.Slots.Count < 1)
                return;
            
            hasClickedOnSelect = true;
        }
        
        private bool IsSelectedCardAlreadyInDeck()
        {
            if (selectionContainer.Slots.Count < 1 || selectionContainer.Slots[0].CurrentCard == null)
            {
                Debug.LogError($"[{nameof(RewardLoop)}] error : no card selected, how did you do that ?");
                return false;
            }
            
            CardData selectedData =  selectionContainer.Slots[0].CurrentCard.cardController.cardData;
            return PlayerDeck.instance.ContainsCard(selectedData);
        }

        private IEnumerator HealDeckCard()
        {
            CardController healer = selectionContainer.Slots[0].CurrentCard.cardController;
            CardController target = FindCardToHeal(healer.cardData);
            
            if (target == null)
            {
                Debug.LogError($"[{nameof(RewardLoop)}] error : selected card not found, could not heal, how did you do that ?");
                yield break;
            }

            yield return CardTween.PlayPhysicalAttack(healer, target);
            healer.KillCard();

            target.cardHealth.Heal(target.cardData.hpMax);
            PlayerDeck.instance.UpdateCardHealthPoints(target.deckCard, target.cardHealth.currentHealth);
        }

        private CardController FindCardToHeal(CardData data)
        {
            foreach (Slot slot in handContainer.Slots)
            {
                if (slot.CurrentCard.cardController.cardData.cardName == data.cardName)
                    return slot.CurrentCard.cardController;
            }

            return null;
        }
        
        private IEnumerator StoreSelectedCard()
        {
            selectionContainer.SendCardToOtherBoard(0, selectedCardsContainer);
            yield return new WaitForSeconds(0.3f);
        }

        private IEnumerator RemoveRemainingCards()
        {
            while (mainContainer.Slots.Count > 0)
            {
                mainContainer.Slots[0].CurrentCard.cardController.KillCard();
                yield return new WaitForSeconds(0.1f);
            }
        }

        private IEnumerator DisplayFinalSelection()
        {
            while (selectedCardsContainer.Slots.Count > 0)
            {
                selectedCardsContainer.SendCardToOtherBoard(0, mainContainer);
                yield return new WaitForSeconds(0.25f);
            }
        }

        private IEnumerator WaitUntilFinalValidation()
        {
            hasClickedOnValidate = false;
            yield return SetValidateButtonState(true);
            yield return new WaitUntil(() => hasClickedOnValidate);
            yield return SendSelectedCardsToHand();
            yield return SetValidateButtonState(false);
        }

        private IEnumerator SetValidateButtonState(bool state)
        {
            validateButton.SetActive(state);
            yield break;
        }

        private bool hasClickedOnValidate;
        public void OnClickValidate()
        {
            hasClickedOnValidate = true;
        }
        
        private IEnumerator SendSelectedCardsToHand()
        {
            while (mainContainer.Slots.Count > 0)
            {
                PlayerDeck.instance.AddCardToDeck(mainContainer.Slots[0].CurrentCard.cardController.cardData);
                
                mainContainer.SendCardToOtherBoard(0, handContainer);
                yield return new WaitForSeconds(0.25f);
            }
            
            while (selectionContainer.Slots.Count > 0)
            {
                PlayerDeck.instance.AddCardToDeck(selectionContainer.Slots[0].CurrentCard.cardController.cardData);
                
                selectionContainer.SendCardToOtherBoard(0, handContainer);
                yield return new WaitForSeconds(0.25f);
            }

            isRewardScreenOver = true;
        }
    }
}
