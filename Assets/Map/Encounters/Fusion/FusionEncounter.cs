using Cards.Scripts;
using Combat.Card_Container.Script;
using Combat.Spells;
using Map.Encounters;
using Map.Encounters.Fusion.Spell_Button_Toggle;
using PrimeTween;
using System;
using System.Collections;
using System.Collections.Generic;
using Combat.Passives;
using TMPro;
using UnityEngine;

public class FusionEncounter : BasicEncounterInteraction
{
    [SerializeField] private List<CardContainer> cardContainers;
    [SerializeField] private Transform buttonsContainer;
    [SerializeField] private GameObject validationButton;
    [SerializeField] private TextMeshProUGUI instructionsText;

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
    }

    private IEnumerator SelectTwoSpells()
    {
        ChangeText(SelectSpellsText);
        TryDisplayValidationButton(false);

        CardContainer card1 = cardContainers[0];
        CardContainer card2 = cardContainers[1];

        Sequence sequence = Sequence.Create()
            .Group(Tween.UIAnchoredPositionY(card1.rectTransform, card1.rectTransform.anchoredPosition.y - 350f, 0.3f))
            .Group(Tween.UIAnchoredPositionX(card1.rectTransform, card1.rectTransform.anchoredPosition.x - 100f, 0.3f))

            .Group(Tween.UIAnchoredPositionY(card2.rectTransform, card2.rectTransform.anchoredPosition.y - 350f, 0.3f))
            .Group(Tween.UIAnchoredPositionX(card2.rectTransform, card2.rectTransform.anchoredPosition.x + 100f, 0.3f))

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

        foreach (CardContainer container in cardContainers)
        {
            if (container.slotCount > 0)
            {
                CardController card = container.Slots[0].CurrentCard.cardController;

            }
        }

        yield break;
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