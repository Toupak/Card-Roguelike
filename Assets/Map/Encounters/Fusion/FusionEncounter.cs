using Cards.Scripts;
using Cards.Tween_Animations;
using Combat.Card_Container.Script;
using Map.Encounters;
using Run_Loop;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FusionEncounter : BasicEncounterInteraction
{
    [SerializeField] private List<CardContainer> fusionContainers;
    //[SerializeField] private List<SpellButton> spellContainers;
    //[SerializeField] private List<PassiveIcons> passiveContainers;

    [SerializeField] private TextMeshProUGUI instructionsText;

    [SerializeField] private string SelectCardsText;
    [SerializeField] private string SelectSpellsText;
    [SerializeField] private string SelectPassiveText;
    [SerializeField] private string SelectArtWorkText;
    [SerializeField] private string SelectNameText;

    protected override IEnumerator DoStuffPreValidation()
    {
        instructionsText.text = SelectCardsText;

        isSelectionValidated = false;
        yield return new WaitUntil(() => isSelectionValidated);
        yield return SelectTwoCards();

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
    private IEnumerator SelectTwoCards()
    {
        yield return null;
    }
    private IEnumerator SelectTwoSpells()
    {
        foreach (CardContainer container in fusionContainers)
        {
            if (container.slotCount > 0)
            {
                CardController card = container.Slots[0].CurrentCard.cardController;
                //ExtractSpells(card);
            }
        }

        yield return null;
    }

    private IEnumerator SelectPassive()
    {
        yield return null;
    }

    private IEnumerator SelectArtWork()
    {
        yield return null;
    }

    private IEnumerator SelectName()
    {
        yield return null;
    }
}
