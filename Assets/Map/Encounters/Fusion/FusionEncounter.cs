using Cards.Scripts;
using Combat.Card_Container.Script;
using PrimeTween;
using Combat.Spells;
using Map.Encounters;
using Map.Encounters.Fusion.Spell_Button_Toggle;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FusionEncounter : BasicEncounterInteraction
{
    [SerializeField] private List<CardContainer> cardContainers;

    [SerializeField] private Transform spellContainer;
    [SerializeField] private DisplaySpellToggleTooltipOnHover spellButtonPrefab;

    [SerializeField] private GameObject validationButton;
    //[SerializeField] private List<PassiveIcons> passiveContainers;

    [SerializeField] private TextMeshProUGUI instructionsText;

    [SerializeField] private string SelectCardsText;
    [SerializeField] private string SelectSpellsText;
    [SerializeField] private string SelectPassiveText;
    [SerializeField] private string SelectArtWorkText;
    [SerializeField] private string SelectNameText;

    protected override void Start()
    {
        base.Start();

        CardContainer.OnAnyContainerUpdated.AddListener(() => TryDisplayValidateButton(CheckContainers()));
    }

    protected override IEnumerator DoStuffPreValidation()
    {
        instructionsText.text = SelectCardsText;

        isSelectionValidated = false;
        yield return new WaitUntil(() => isSelectionValidated);

        yield return SelectTwoSpells();
        isSelectionValidated = false;
        yield return new WaitUntil(() => isSelectionValidated);

        yield return SelectPassive();
        isSelectionValidated = false;
        yield return new WaitUntil(() => isSelectionValidated);

        yield return SelectArtWork();
        isSelectionValidated = false;
        yield return new WaitUntil(() => isSelectionValidated);

        yield return SelectName();
        isSelectionValidated = false;
        yield return new WaitUntil(() => isSelectionValidated);
    }

    private IEnumerator SelectTwoSpells()
    {
        instructionsText.text = SelectSpellsText;
        validationButton.SetActive(false);

        var card1 = cardContainers[0].GetComponent<RectTransform>();
        var card2 = cardContainers[1].GetComponent<RectTransform>();

        Sequence sequence = Sequence.Create()
            .Group(Tween.UIAnchoredPositionX(card1, card1.anchoredPosition.x - 250f, 0.3f))
            .Group(Tween.UIAnchoredPositionX(card2, card2.anchoredPosition.x + 250f, 0.3f));

        yield return sequence;

        foreach (CardContainer container in cardContainers)
        {
            if (container.slotCount > 0)
            {
                CardController card = container.Slots[0].CurrentCard.cardController;

                if (card.singleButton.spellController != null)
                {
                    DisplaySpellToggleTooltipOnHover spell = Instantiate(spellButtonPrefab, spellContainer);
                    spell.Setup(card, card.singleButton.spellData);
                }

                if (card.leftButton.spellController != null)
                {
                    DisplaySpellToggleTooltipOnHover spell2 = Instantiate(spellButtonPrefab, spellContainer);
                    spell2.Setup(card, card.leftButton.spellData);
                }

                if (card.rightButton.spellController != null)
                {
                    DisplaySpellToggleTooltipOnHover spell3 = Instantiate(spellButtonPrefab, spellContainer);
                    spell3.Setup(card, card.rightButton.spellData);
                }
            }
        }

        yield return null;
    }

    private IEnumerator SelectPassive()
    {
        instructionsText.text = SelectPassiveText;
        yield return null;
    }

    private IEnumerator SelectArtWork()
    {
        instructionsText.text = SelectArtWorkText;
        yield return null;
    }

    private IEnumerator SelectName()
    {
        instructionsText.text = SelectNameText;
        yield return null;
    }

    private bool CheckContainers()
    {
        bool isAnyCardContainersEmpty = false;

        foreach (CardContainer container in cardContainers)
        {
            if (container.slotCount == 0)
                isAnyCardContainersEmpty = true;
        }

        return !isAnyCardContainersEmpty;
    }

    private void TryDisplayValidateButton(bool conditionToCheck)
    {
         validationButton.SetActive(conditionToCheck);
    }
}