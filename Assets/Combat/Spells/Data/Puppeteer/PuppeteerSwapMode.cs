using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using Cards.Scripts;
using Combat.Passives;
using UnityEngine;

namespace Combat.Spells.Data.Puppeteer
{
    public class PuppeteerSwapMode : SpellController
    {
        [SerializeField] private Sprite puppetOffensive;
        [SerializeField] private Sprite puppetDefensive;

        [Space] 
        [SerializeField] private PassiveData offensiveFriendPassive;
        [SerializeField] private PassiveData offensiveEnemyPassive;
        [SerializeField] private PassiveData defensiveFriendPassive;
        [SerializeField] private PassiveData defensiveEnemyPassive;
        
        public bool isPuppetInOffensiveMode { get; private set; } = true;
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            isPuppetInOffensiveMode = !isPuppetInOffensiveMode;
            CardController puppet = cardController.leftButton.spellController.GetComponent<PuppeteerMove>().puppetCard;
            UpdatePuppetMode(puppet);
        }

        public void UpdatePuppetMode(CardController puppetCard)
        {
            bool isEnemy = puppetCard.tokenParentController.isEnemy;

            puppetCard.SetArtwork(isPuppetInOffensiveMode ? puppetOffensive : puppetDefensive);
            puppetCard.passiveHolder.RemoveAllPassives();
            puppetCard.passiveHolder.AddPassive(ComputePassive(isPuppetInOffensiveMode, isEnemy));
        }

        private PassiveData ComputePassive(bool isOffensive, bool isEnemy)
        {
            if (isOffensive)
                return isEnemy ? offensiveEnemyPassive : offensiveFriendPassive;
            else
                return isEnemy ? defensiveEnemyPassive : defensiveFriendPassive;
        }
    }
}
