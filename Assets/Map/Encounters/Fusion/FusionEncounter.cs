using Cards.Scripts;
using Combat.Card_Container.Script;
using Combat.Spells;
using Map.Encounters;
using Map.Encounters.Fusion.Spell_Button_Toggle;
using PrimeTween;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FusionEncounter : BasicEncounterInteraction
{
    [SerializeField] private List<CardContainer> cardContainers;
    [SerializeField] private Transform buttonsContainer;
    [SerializeField] private GameObject validationButton;
    [SerializeField] private TextMeshProUGUI instructionsText;

    //Spells
    [SerializeField] private DisplaySpellToggleTooltipOnHover spellButtonPrefab;
    private List<FusionToggleButton> spellButtons = new();
    private SpellData leftSpellData = null;
    private SpellData rightSpellData;

    //[SerializeField] private List<PassiveIcons> passiveContainers;



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

    protected override void Start()
    {
        base.Start();

        TryDisplayValidationButton(false);
        CardContainer.OnAnyContainerUpdated.AddListener(() => TryDisplayValidationButton(CheckCardContainers()));
    }

    protected override IEnumerator DoStuffPreValidation()
    {
        ChangeText(SelectCardsText);

        isSelectionValidated = false;
        yield return new WaitUntil(() => isSelectionValidated);

        yield return SelectTwoSpells();
        isSelectionValidated = false;
        yield return new WaitUntil(() => isSelectionValidated);

        //DoTransition();
        yield return StoreAndRemoveSpellButtons();

        yield return SelectPassive();
        isSelectionValidated = false;
        yield return new WaitUntil(() => isSelectionValidated);

        //DoTransition();
        yield return SelectArtWork();
        isSelectionValidated = false;
        yield return new WaitUntil(() => isSelectionValidated);

        //DoTransition();
        yield return SelectName();
        isSelectionValidated = false;
        yield return new WaitUntil(() => isSelectionValidated);
    }

    private IEnumerator SelectTwoSpells()
    {
        ChangeText(SelectSpellsText);
        TryDisplayValidationButton(false);

        var card1 = cardContainers[0].GetComponent<RectTransform>();
        var card2 = cardContainers[1].GetComponent<RectTransform>();
        var rectHandContainer = handContainer.GetComponent<RectTransform>();

        Sequence sequence = Sequence.Create()
            .Group(Tween.UIAnchoredPositionY(card1, card1.anchoredPosition.y - 350f, 0.3f))
            .Group(Tween.UIAnchoredPositionX(card1, card1.anchoredPosition.x - 100f, 0.3f))

            .Group(Tween.UIAnchoredPositionY(card2, card2.anchoredPosition.y - 350f, 0.3f))
            .Group(Tween.UIAnchoredPositionX(card2, card2.anchoredPosition.x + 100f, 0.3f))

            .Group(Tween.UIAnchoredPositionY(rectHandContainer, rectHandContainer.anchoredPosition.y - 250f, 0.3f));

        yield return sequence;

        foreach (CardContainer container in cardContainers)
        {
            if (container.slotCount > 0)
            {
                CardController card = container.Slots[0].CurrentCard.cardController;

                if (card.singleButton.spellController != null)
                {
                    DisplaySpellToggleTooltipOnHover spell = Instantiate(spellButtonPrefab, buttonsContainer);
                    spell.Setup(card, card.singleButton.spellData);
                    
                    FusionToggleButton spellToggle = spell.GetComponent<FusionToggleButton>();
                    spellToggle.OnClick.AddListener(() => TryDisplayValidationButton(CheckSpellContainers()));
                    spellButtons.Add(spellToggle);
                }

                if (card.leftButton.spellController != null)
                {
                    DisplaySpellToggleTooltipOnHover spell2 = Instantiate(spellButtonPrefab, buttonsContainer);
                    spell2.Setup(card, card.leftButton.spellData);

                    FusionToggleButton spellToggle = spell2.GetComponent<FusionToggleButton>();
                    spellToggle.OnClick.AddListener(() => TryDisplayValidationButton(CheckSpellContainers()));
                    spellButtons.Add(spellToggle);
                }

                if (card.rightButton.spellController != null)
                {
                    DisplaySpellToggleTooltipOnHover spell3 = Instantiate(spellButtonPrefab, buttonsContainer);
                    spell3.Setup(card, card.rightButton.spellData);
                    
                    FusionToggleButton spellToggle = spell3.GetComponent<FusionToggleButton>();
                    spellToggle.OnClick.AddListener(() => TryDisplayValidationButton(CheckSpellContainers()));
                    spellButtons.Add(spellToggle);
                }
            }
        }
    }
    private IEnumerator StoreAndRemoveSpellButtons()
    {
        foreach (FusionToggleButton button in spellButtons)
        {
            if (button.isOn)
            {
                if (leftSpellData == null)
                    leftSpellData = button.GetComponent<DisplaySpellToggleTooltipOnHover>().currentSpellData;
                else
                    rightSpellData = button.GetComponent<DisplaySpellToggleTooltipOnHover>().currentSpellData;
            }

            Destroy(button.gameObject);
        }

        yield return null;
    }

    private IEnumerator SelectPassive()
    {
        ChangeText(SelectPassiveText);
        yield return null;
    }

    private IEnumerator SelectArtWork()
    {
        ChangeText(SelectArtWorkText);
        yield return null;
    }

    private IEnumerator SelectName()
    {
        ChangeText(SelectNameText);
        yield return null;
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

    private bool CheckSpellContainers()
    {
        int count = 0;
        foreach (FusionToggleButton button in spellButtons)
        {
            if (button.isOn)
                count += 1;
        }

        if (count >= 3)
            ChangeText(TooManySpellsSelected);

        if (count < 2)
            ChangeText(SelectSpellsText);

        if (count == 2)
            ChangeText(TwoSpellsSelected);

        return count == 2;
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