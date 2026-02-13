using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BoomLib.Tools;
using Cards.Scripts;
using Cards.Tween_Animations;
using Combat.Card_Container.CardSlot;
using Combat.Card_Container.Script;
using Inventory.Drop_Rates;
using Inventory.Items;
using Inventory.Items.Frames;
using UnityEngine;

namespace Run_Loop
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

        public bool isRewardScreenOver { get; private set; }
        
        private void Awake()
        {
            instance = this;
        }

        private IEnumerator Start()
        {
            //FillDeckForTest();

            bool isFirstRun = IsFirstRun();
            
            if (!isFirstRun)
                yield return LoadCurrentDeckInHand();

            yield return OpenBooster(3);
            
            if (DropRateManager.instance.CheckForFrameReward())
                yield return OpenFrameBooster();

            yield return DisplayFinalSelection();
            yield return WaitUntilFinalValidation();
        }

        private IEnumerator OpenFrameBooster()
        {
            yield return WaitUntilOpenButtonIsClicked();
            yield return OpenBoosterAndDisplayFrames(5);
            yield return WaitUntilCardHasBeenSelected();
            yield return StoreSelectedCard();
            yield return RemoveRemainingCards();
        }

        private IEnumerator OpenBooster(int cardCount)
        {
            yield return WaitUntilOpenButtonIsClicked();
            yield return OpenBoosterAndDisplayCards(cardCount);
            yield return WaitUntilCardHasBeenSelected();
            if (IsSelectedCardAlreadyInDeck())
                yield return HealDeckCard();
            else
                yield return StoreSelectedCard();
            yield return RemoveRemainingCards();
        }

        private void FillDeckForTest()
        {
            foreach (CardData data in testData)
            {
                PlayerDeck.instance.AddCardToDeck(data);
            }
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
                yield return new WaitForSeconds(0.05f);
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
        
        private IEnumerator OpenBoosterAndDisplayFrames(int framesCount)
        {
            List<FrameItem> alreadyOwnedFrames = PlayerInventory.instance.frames;
            
            List<FrameData> commonFrames = RunLoop.instance.framesData.Where((c) => c.isLowRarity && alreadyOwnedFrames.Count((d) => d.data.frameName == c.frameName) < 1).ToList();
            if (commonFrames.Count > 0)
                commonFrames.Shuffle();
            
            List<FrameData> legendaryFrames = RunLoop.instance.framesData.Where((c) => c.rarity == CardData.Rarity.Legendary && alreadyOwnedFrames.Count((d) => d.data.frameName == c.frameName) < 1).ToList();
            if (legendaryFrames.Count > 0)
                legendaryFrames.Shuffle();
            
            List<FrameData> exoticFrames = RunLoop.instance.framesData.Where((c) => c.rarity == CardData.Rarity.Exotic && alreadyOwnedFrames.Count((d) => d.data.frameName == c.frameName) < 1).ToList();
            if (exoticFrames.Count > 0)
                exoticFrames.Shuffle();

            int commonIndex = 0;
            int legendaryIndex = 0;
            int exoticIndex = 0;
            for (int i = 0; i < framesCount; i++)
            {
                if (exoticIndex < exoticFrames.Count && DropRateManager.instance.CheckForExoticFrameReward())
                    DrawItemToContainer(mainContainer).SetupAsFrameItem(exoticFrames[exoticIndex++]);
                else if (legendaryIndex < legendaryFrames.Count && DropRateManager.instance.CheckForLegendaryFrameReward())
                    DrawItemToContainer(mainContainer).SetupAsFrameItem(legendaryFrames[legendaryIndex++]);
                else if (commonIndex < commonFrames.Count)
                    DrawItemToContainer(mainContainer).SetupAsFrameItem(commonFrames[commonIndex++]);
                yield return new WaitForSeconds(0.1f);
            }
        }
        
        private IEnumerator OpenBoosterAndDisplayCards(int cardCount)
        {
            List<CardData> commonCards = RunLoop.instance.dataBase.GetAllCards((c) => c.canBeDrawn && c.isLowRarity);
            if (commonCards == null || commonCards.Count < 1)
                Debug.LogError($"[{nameof(RewardLoop)}] error : no Common and Rare cards found in dataBase");
            commonCards.Shuffle();
            
            List<CardData> legendaryCards = RunLoop.instance.dataBase.GetAllCards((c) => c.canBeDrawn && c.rarity == CardData.Rarity.Legendary);
            if (legendaryCards == null || legendaryCards.Count < 1)
                Debug.LogError($"[{nameof(RewardLoop)}] error : no Legendary cards found in dataBase");
            legendaryCards.Shuffle();
            
            List<CardData> exoticCards = RunLoop.instance.dataBase.GetAllCards((c) => c.canBeDrawn && c.rarity == CardData.Rarity.Exotic);
            if (exoticCards == null || exoticCards.Count < 1)
                Debug.LogError($"[{nameof(RewardLoop)}] error : no Exotic cards found in dataBase");
            exoticCards.Shuffle();

            int commonIndex = 0;
            int legendaryIndex = 0;
            int exoticIndex = 0;
            for (int i = 0; i < cardCount; i++)
            {
                if (exoticCards != null && exoticIndex < exoticCards.Count && DropRateManager.instance.CheckForExoticCardReward())
                    DrawCardToContainer(new DeckCard(exoticCards[exoticIndex++]), mainContainer);
                else if (legendaryCards != null && legendaryIndex < legendaryCards.Count && DropRateManager.instance.CheckForLegendaryCardReward())
                    DrawCardToContainer(new DeckCard(legendaryCards[legendaryIndex++]), mainContainer);
                else if (commonCards != null && commonIndex < commonCards.Count)
                    DrawCardToContainer(new DeckCard(commonCards[commonIndex++]), mainContainer);
                yield return new WaitForSeconds(0.1f);
            }
        }
        
        private void DrawCardToContainer(DeckCard card, CardContainer container)
        {
            CardMovement newCard = Instantiate(cardMovementPrefab);
            container.ReceiveCard(newCard);
            
            CardController controller = CardsVisualManager.instance.SpawnNewCardVisuals(newCard, card);
            newCard.SetCardController(controller);
        }
        
        private ItemController DrawItemToContainer(CardContainer container)
        {
            CardMovement newCard = Instantiate(cardMovementPrefab);
            container.ReceiveCard(newCard);
            
            ItemController controller = CardsVisualManager.instance.SpawnNewItemVisuals(newCard);
            newCard.SetItemController(controller);

            return controller;
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
            healer.KillCard(false);

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
                mainContainer.Slots[0].CurrentCard.KillCard(false);
                yield return new WaitForSeconds(0.1f);
            }
        }

        private IEnumerator DisplayFinalSelection()
        {
            while (selectedCardsContainer.Slots.Count > 0)
            {
                selectedCardsContainer.SendCardToOtherBoard(0, mainContainer);
                yield return new WaitForSeconds(0.1f);
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
                if (mainContainer.Slots[0].CurrentCard.cardController != null)
                {
                    PlayerDeck.instance.AddCardToDeck(mainContainer.Slots[0].CurrentCard.cardController.cardData);
                    mainContainer.SendCardToOtherBoard(0, handContainer);
                }
                else
                {
                    FrameCardItem frameCardItem = mainContainer.Slots[0].CurrentCard.itemController.GetComponent<FrameCardItem>();

                    if (frameCardItem != null)
                        PlayerInventory.instance.LootFrame(frameCardItem.data);

                    mainContainer.Slots[0].CurrentCard.KillCard();
                }
                
                yield return new WaitForSeconds(0.25f);
            }
            
            while (selectionContainer.Slots.Count > 0)
            {
                if (selectionContainer.Slots[0].CurrentCard.cardController != null)
                {
                    PlayerDeck.instance.AddCardToDeck(selectionContainer.Slots[0].CurrentCard.cardController.cardData);
                    selectionContainer.SendCardToOtherBoard(0, handContainer);
                }
                else 
                    selectionContainer.Slots[0].CurrentCard.KillCard();
                
                yield return new WaitForSeconds(0.25f);
            }

            isRewardScreenOver = true;
        }
    }
}
