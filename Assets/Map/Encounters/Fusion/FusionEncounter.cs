using Cards.Scripts;
using Cards.Tween_Animations;
using Combat.Card_Container.Script;
using Combat.Spells;
using Map.Encounters;
using Map.Encounters.Fusion.Spell_Button_Toggle;
using Run_Loop;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

public class FusionEncounter : BasicEncounterInteraction
{
    [SerializeField] private List<CardContainer> fusionContainers;

    [SerializeField] private List<DisplaySpellToggleTooltipOnHover> spellContainers;
    private int spellLimit = 4;

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
        //Checker combien il y a de spells au total dans les deux cartes et les ajoute dans une liste de spells data
        //Créer le bon montant de prefabs
        //recrache la liste des spells data en faisant un .Setup() sur les prefab avec chacun des spells

        //Need vérifier que deux spells ou la max de la liste sont bien cliqués et qu'on puisse pas en cliquer plus
        //Ok pour valider 

        instructionsText.text = SelectSpellsText;
        int spellCount = 0;
        foreach (CardContainer container in fusionContainers)
        {
            for (int i = spellCount; spellCount < spellLimit; spellCount++)
            {
                if (container.slotCount > 0)
                {
                    CardController card = container.Slots[0].CurrentCard.cardController;
                    spellContainers[i].GetComponent<DisplaySpellToggleTooltipOnHover>().Setup(card, card.leftButton.spellData);
                    spellContainers[i].GetComponent<DisplaySpellToggleTooltipOnHover>().Setup(card, card.rightButton.spellData);
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
}
