using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using Cards.Scripts;
using Combat.Passives;
using UnityEngine;

namespace Combat.Spells.Data.SpiderBall
{
    public class SpiderBallSwap : SpellController
    {
        [SerializeField] private SpellData strikeData;
        [SerializeField] private SpellData spareData;
        
        [Space]
        [SerializeField] private PassiveData ballPassiveData;
        [SerializeField] private PassiveData spiderPassiveData;

        [Space]
        [SerializeField] private Sprite ballSprite;
        [SerializeField] private Sprite spiderSprite;

        private bool isInBallForm = true;
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            isInBallForm = !isInBallForm;
            
            cardController.passiveHolder.RemovePassive(isInBallForm ? spiderPassiveData : ballPassiveData);
            cardController.passiveHolder.AddPassive(isInBallForm ? ballPassiveData : spiderPassiveData);
            cardController.SetupLeftSpell(isInBallForm ? strikeData : spareData);
            cardController.SetArtwork(isInBallForm ? ballSprite : spiderSprite);
        }
    }
}
