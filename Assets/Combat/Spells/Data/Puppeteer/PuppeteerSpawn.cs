using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Cards.Tween_Animations;
using UnityEngine;

namespace Combat.Spells.Data.Puppeteer
{
    public class PuppeteerSpawn : SpellController
    {
        [SerializeField] private CardData puppetData;
        
        [Space]
        [SerializeField] private SpellData leftSpell;
        [SerializeField] private SpellData rightSpell;

        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            yield return CardTween.NewPlaySelfAction(cardController);
                
            SpawnCardGA spawnCardGa = new SpawnCardGA(puppetData, cardController, true);
            ActionSystem.instance.Perform(spawnCardGa);
            
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            

            cardController.SetupLeftSpell(leftSpell);
            cardController.SetupRightSpell(rightSpell);
            cardController.leftButton.spellController.GetComponent<PuppeteerMove>().puppetCard = spawnCardGa.spawnedCard;

            ActionSystem.instance.SetLock(false);
            isLocking = false;
        }
    }
}
