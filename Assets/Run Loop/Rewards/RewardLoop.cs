using System.Collections;
using System.Collections.Generic;
using Board.Script;
using Cards.Scripts;
using CardSlot.Script;
using Run_Loop.Run_Parameters;
using UnityEngine;

namespace Run_Loop.Rewards
{
    public class RewardLoop : MonoBehaviour
    {
        [SerializeField] private CardContainer mainContainer;
        [SerializeField] private CardContainer handContainer;
        [SerializeField] private CardContainer selectionContainer;
        [SerializeField] private GameObject selectionSquare;
        
        [Space]
        [SerializeField] private GameObject openBoosterButton;
        [SerializeField] private GameObject selectCardButton;
        [SerializeField] private GameObject validateButton;
        
        [Space]
        [SerializeField] private CardMovement cardMovementPrefab;
        
        public static RewardLoop instance;

        private RunParameterData runParameterData;
        
        private void Awake()
        {
            instance = this;
        }

        private IEnumerator Start()
        {
            runParameterData = ComputeRunParameterData();

            yield return LoadCurrentDeckInHand();

            for (int i = 0; i < runParameterData.boosterCount; i++)
            {
                yield return WaitUntilOpenButtonIsClicked();
                yield return OpenBoosterAndDisplayCards();
                yield return WaitUntilCardHasBeenSelected();
                yield return StoreSelectedCard();
                yield return RemoveRemainingCards();
            }

            yield return DisplayFinalSelection();
            yield return WaitUntilFinalValidation();
        }

        private RunParameterData ComputeRunParameterData()
        {
            if (RunLoop.instance != null && RunLoop.instance.currentRunParameterData != null)
                return RunLoop.instance.currentRunParameterData;
            else
                return new RunParameterData(3, 3, 3);
        }
        
        private IEnumerator LoadCurrentDeckInHand()
        {
            yield break;
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
        
        private IEnumerator OpenBoosterAndDisplayCards()
        {
            for (int i = 0; i < runParameterData.cardCount; i++)
            {
                DrawCard();
                yield return new WaitForSeconds(0.3f);
            }
        }
        
        private void DrawCard()
        {
            CardMovement newCard = Instantiate(cardMovementPrefab);
            mainContainer.ReceiveCard(newCard);

            CardData cardData = RunLoop.instance.dataBase.GetRandomCard((c) => !c.isEnemy);
            CardController controller = CardsVisualManager.instance.SpawnNewCardVisuals(newCard, new DeckCard(cardData));
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
        
        private IEnumerator StoreSelectedCard()
        {
            selectionContainer.SendCardToOtherBoard(0, handContainer);
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
            while (handContainer.Slots.Count > 0)
            {
                handContainer.SendCardToOtherBoard(0, mainContainer);
                yield return new WaitForSeconds(0.25f);
            }
            
            selectionSquare.SetActive(false);
        }

        private IEnumerator WaitUntilFinalValidation()
        {
            hasClickedOnValidate = false;
            yield return SetValidateButtonState(true);
            yield return new WaitUntil(() => hasClickedOnValidate);
            yield return SetValidateButtonState(false);
        }
        
        private IEnumerator SetValidateButtonState(bool state)
        {
            validateButton.SetActive(state);
            yield break;
        }

        public bool hasClickedOnValidate { get; private set; }
        public void OnClickValidate()
        {
            hasClickedOnValidate = true;
        }

        public List<CardData> RetrieveSelectedCards()
        {
            List<CardData> selectedCards = new List<CardData>();

            foreach (Slot slot in mainContainer.Slots)
            {
                selectedCards.Add(slot.CurrentCard.cardController.cardData);
            }

            return selectedCards;
        }
    }
}
