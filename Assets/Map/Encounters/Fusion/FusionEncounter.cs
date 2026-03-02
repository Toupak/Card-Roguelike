using BoomLib.Tools;
using Cards.Scripts;
using Combat.Card_Container.Script;
using Combat.Passives;
using Combat.Spells;
using Cursor.Script;
using Localization;
using Map.Encounters;
using Map.Encounters.Fusion.Spell_Button_Toggle;
using Map.Rooms;
using PrimeTween;
using Run_Loop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class FusionEncounter : BasicEncounterInteraction
{
    private bool isUsed;
    [SerializeField] private Animator animator;

    [SerializeField] private List<CardContainer> cardContainers;
    [SerializeField] private Transform buttonsContainer;
    [SerializeField] private GameObject validationButton;
    [SerializeField] private TextMeshProUGUI instructionsText;

    [SerializeField] private CardData fusionCardData;
    private CardData newCard;

    private CardController card1;
    private CardController card2;

    //Spells
    [SerializeField] private DisplayToggleTooltipOnHover spellButtonPrefab;
    private List<FusionToggleButton> ToggleButtons = new();
    private SpellData leftSpellData = null;
    private SpellData rightSpellData;
    private const int maxSpellQuantity = 2;

    //Passive
    [SerializeField] private DisplayToggleTooltipOnHover passiveButtonPrefab;
    private const int maxPassiveCount = 1;
    private PassiveData passiveData;

    //ArtWork
    private Sprite artworkSelected;
    private bool isSelectingArtwork;
    private CardController currentCardSelected;

    //Name
    [SerializeField] private TMP_InputField inputField;
    private string newCardName;

    //Texts
    [Header ("Instruction")]
    [SerializeField] private string SelectCardsText;
    [SerializeField] private string SelectSpellsText;
    [SerializeField] private string SelectPassiveText;
    [SerializeField] private string SelectArtWorkText;
    [SerializeField] private string SelectNameText;

    [Header ("Interaction Text")]
    [SerializeField] private string TooManySpellsSelected;
    [SerializeField] private string TwoSpellsSelected;

    [SerializeField] private string TooManyPassivesSelected;
    [SerializeField] private string OnePassiveSelected;

    [SerializeField] private string TooManyArtworksSelected;
    [SerializeField] private string OneArtworksSelected;

    protected override void Start()
    {
        base.Start();

        TryDisplayValidationButton(false);
        CardContainer.OnAnyContainerUpdated.AddListener(() => TryDisplayValidationButton(CheckCardContainers()));
    }

    private void Update()
    {
        if (isSelectingArtwork && Mouse.current.leftButton.wasPressedThisFrame)
            TryRegisterCardController();
    }

    public override bool CanInteract()
    {
        return !isUsed;
    }

    protected override void Setup()
    {
        base.Setup();

        isUsed = RoomBuilder.instance.HasRoomBeenCleared();
        if (isUsed)
            animator.Play("Used");
    }

    protected override IEnumerator DoStuffPreValidation()
    {
        ChangeText(SelectCardsText);

        isSelectionValidated = false;
        yield return new WaitUntil(() => isSelectionValidated);

        yield return SelectTwoSpells();
        isSelectionValidated = false;
        yield return new WaitUntil(() => isSelectionValidated);

        yield return StoreAndRemoveToggleButtons(true);
        //DoSpellTransition();

        yield return SelectPassive();
        isSelectionValidated = false;
        yield return new WaitUntil(() => isSelectionValidated);


        yield return StoreAndRemoveToggleButtons(false);
        //DoPassiveTransition();

        yield return SelectArtWork();
        isSelectionValidated = false;
        yield return new WaitUntil(() => isSelectionValidated);

        //DoTransition();
        yield return SelectName();
        isSelectionValidated = false;
        yield return new WaitUntil(() => isSelectionValidated);

        CreateCard();
    }

    private IEnumerator SelectTwoSpells()
    {
        ChangeText(SelectSpellsText);
        TryDisplayValidationButton(false);

        CardContainer cardContainer1 = cardContainers[0];
        CardContainer cardContainer2 = cardContainers[1];

        card1 = cardContainer1.Slots[0].CurrentCard.cardController;
        card2 = cardContainer2.Slots[0].CurrentCard.cardController;

        Sequence sequence = Sequence.Create()
            .Group(Tween.UIAnchoredPositionY(cardContainer1.rectTransform, cardContainer1.rectTransform.anchoredPosition.y - 350f, 0.3f))
            .Group(Tween.UIAnchoredPositionX(cardContainer1.rectTransform, cardContainer1.rectTransform.anchoredPosition.x - 100f, 0.3f))

            .Group(Tween.UIAnchoredPositionY(cardContainer2.rectTransform, cardContainer2.rectTransform.anchoredPosition.y - 350f, 0.3f))
            .Group(Tween.UIAnchoredPositionX(cardContainer2.rectTransform, cardContainer2.rectTransform.anchoredPosition.x + 100f, 0.3f))

            .Group(Tween.UIAnchoredPositionY(handContainer.rectTransform, handContainer.rectTransform.anchoredPosition.y - 250f, 0.3f));

        yield return sequence;

        foreach (CardContainer container in cardContainers)
        {
            if (container.slotCount > 0)
            {
                CardController card = container.Slots[0].CurrentCard.cardController;

                if (card.singleButton.spellController != null)
                {
                    DisplayToggleTooltipOnHover spell = Instantiate(spellButtonPrefab, buttonsContainer);
                    spell.Setup(card, card.singleButton.spellData);
                    
                    FusionToggleButton spellToggle = spell.GetComponent<FusionToggleButton>();
                    spellToggle.OnClick.AddListener(() => TryDisplayValidationButton(CheckContainers(true)));
                    ToggleButtons.Add(spellToggle);
                }

                if (card.leftButton.spellController != null)
                {
                    DisplayToggleTooltipOnHover spell2 = Instantiate(spellButtonPrefab, buttonsContainer);
                    spell2.Setup(card, card.leftButton.spellData);

                    FusionToggleButton spellToggle = spell2.GetComponent<FusionToggleButton>();
                    spellToggle.OnClick.AddListener(() => TryDisplayValidationButton(CheckContainers(true)));
                    ToggleButtons.Add(spellToggle);
                }

                if (card.rightButton.spellController != null)
                {
                    DisplayToggleTooltipOnHover spell3 = Instantiate(spellButtonPrefab, buttonsContainer);
                    spell3.Setup(card, card.rightButton.spellData);
                    
                    FusionToggleButton spellToggle = spell3.GetComponent<FusionToggleButton>();
                    spellToggle.OnClick.AddListener(() => TryDisplayValidationButton(CheckContainers(true)));
                    ToggleButtons.Add(spellToggle);
                }
            }
        }
    }

    private IEnumerator SelectPassive()
    {
        ChangeText(SelectPassiveText);
        TryDisplayValidationButton(false);

        foreach (CardContainer container in cardContainers)
        {
            if (container.slotCount > 0)
            {
                CardController card = container.Slots[0].CurrentCard.cardController;

                foreach (PassiveController passiveController in card.passiveHolder.passives)
                {
                    DisplayToggleTooltipOnHover passive = Instantiate(passiveButtonPrefab, buttonsContainer);
                    passive.Setup(card, passiveController.passiveData);
                    
                    FusionToggleButton passiveToggle = passive.GetComponent<FusionToggleButton>();
                    passiveToggle.OnClick.AddListener(() => TryDisplayValidationButton(CheckContainers(false)));
                    ToggleButtons.Add(passiveToggle);
                }
            }
        }

        yield break;
    }

    private IEnumerator StoreAndRemoveToggleButtons(bool isSpellSelected)
    {
        foreach (FusionToggleButton button in ToggleButtons)
        {
            if (button.isOn)
            {
                if (isSpellSelected)
                {
                    if (leftSpellData == null)
                        leftSpellData = button.GetComponent<DisplayToggleTooltipOnHover>().currentSpellData;
                    else
                        rightSpellData = button.GetComponent<DisplayToggleTooltipOnHover>().currentSpellData;
                }
                else
                {
                    passiveData = button.GetComponent<DisplayToggleTooltipOnHover>().currentPassiveData;
                }
            }

            Destroy(button.gameObject);
            Debug.Log("Button is destroyed");
        }

        ToggleButtons.Clear();

        yield break;
    }

    private IEnumerator SelectArtWork()
    {
        ChangeText(SelectArtWorkText);
        TryDisplayValidationButton(false);
        isSelectingArtwork = true;

        CursorInfo.instance.SetCursorMode(CursorInfo.CursorMode.Targeting);

        CardContainer card1 = cardContainers[0];
        CardContainer card2 = cardContainers[1];

        Sequence sequence = Sequence.Create()
            .Group(Tween.UIAnchoredPositionY(card1.rectTransform, card1.rectTransform.anchoredPosition.y + 350f, 0.3f))
            .Group(Tween.UIAnchoredPositionX(card1.rectTransform, card1.rectTransform.anchoredPosition.x + 150f, 0.3f))

            .Group(Tween.UIAnchoredPositionY(card2.rectTransform, card2.rectTransform.anchoredPosition.y + 350f, 0.3f))
            .Group(Tween.UIAnchoredPositionX(card2.rectTransform, card2.rectTransform.anchoredPosition.x - 150f, 0.3f));

        yield break;
    }

    private IEnumerator SelectName()
    {
        CursorInfo.instance.SetCursorMode(CursorInfo.CursorMode.Free);
        isSelectingArtwork = false;
        ChangeText(SelectNameText);

        DisplayFusionCardTemp();

        inputField.gameObject.SetActive(true);

        yield break;
    }

    private void CreateCard()
    {
        if (newCardName != null)
        {
            newCard.cardName = newCardName;
            Debug.Log("Yo depuis le if");
        }


        Debug.Log(newCardName);

        PlayerDeck.instance.RemoveCardFromDeck(card1.cardData);
        PlayerDeck.instance.RemoveCardFromDeck(card2.cardData);

        PlayerDeck.instance.AddCardToDeck(newCard);

        RoomBuilder.instance.MarkCurrentRoomAsCleared();
    }

    public void InputCardName()
    {
        newCardName = inputField.text;
        TryDisplayValidationButton(!string.IsNullOrEmpty(newCardName));
    }

    private void DisplayFusionCardTemp()
    {
        newCard = fusionCardData.Clone();
     
        int hp1 = card1.cardData.hpMax;
        int hp2 = card2.cardData.hpMax;

        int newHpMax = Mathf.Max(hp1, hp2) + (Mathf.Min(hp1, hp2) / 2);
        newCard.hpMax = newHpMax;
        newCard.currentHp = newHpMax;

        if (leftSpellData != null)
            newCard.spellList.Add(leftSpellData);
    
        if (rightSpellData != null)
            newCard.spellList.Add(rightSpellData);

        if (passiveData != null)
            newCard.passiveList.Add(passiveData);

        if (artworkSelected != null)
            newCard.artwork = artworkSelected;

        foreach (CardContainer container in cardContainers)
            container.ResetContainer();

        Destroy(cardContainers[1].gameObject);

        RunLoop.instance.DrawCardToContainer(newCard, cardContainers[0]);
    }

    private bool CheckCardContainers()
    {
        bool isAnyCardContainersEmpty = false;

        foreach (CardContainer container in cardContainers)
        {
            if (container.slotCount == 0)
                isAnyCardContainersEmpty = true;
        }

        return !isAnyCardContainersEmpty;
    }

    private bool CheckContainers(bool isSelectingSpells)
    {
        int maxContainerQuantity = isSelectingSpells ? maxSpellQuantity : maxPassiveCount;

        int count = 0;
        foreach (FusionToggleButton button in ToggleButtons)
        {
            if (button.isOn)
                count += 1;
        }

        if (count > maxContainerQuantity)
            ChangeText(isSelectingSpells ? TooManySpellsSelected : TooManyPassivesSelected);

        if (count < maxContainerQuantity)
            ChangeText(isSelectingSpells ? SelectSpellsText : SelectPassiveText);

        if (count == maxContainerQuantity)
            ChangeText(isSelectingSpells ? TwoSpellsSelected : OnePassiveSelected);

        return count == maxContainerQuantity;
    }

    private void TryRegisterCardController()
    {
        if (CursorInfo.instance.LastCardContainer != null)
        {
            if (currentCardSelected != null)
                currentCardSelected.displayCardEffect.SetPotentialTargetState(false);

            currentCardSelected = CursorInfo.instance.LastCardContainer.Slots[0].CurrentCard.cardController;
            
            artworkSelected = currentCardSelected.cardData.artwork;
            currentCardSelected.displayCardEffect.SetPotentialTargetState(true);
        }

        if (currentCardSelected != null)
            TryDisplayValidationButton(true);

        if (currentCardSelected == null)
            TryDisplayValidationButton(false);
    }

    private void ChangeText(string text)
    {
        instructionsText.text = text;
        //PlaySFX
    }

    private void TryDisplayValidationButton(bool conditionToCheck)
    {
         validationButton.SetActive(conditionToCheck);
    }
}