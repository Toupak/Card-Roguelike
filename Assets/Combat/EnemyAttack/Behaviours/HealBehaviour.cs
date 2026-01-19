using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using CombatLoop;
using Spells;
using Spells.Targeting;
using UnityEngine;

namespace EnemyAttack.Behaviours
{
    public class HealBehaviour : BaseEnemyBehaviour
    {
        [SerializeField] private int healAmount;
        [Space]
        [SerializeField] private float prioritizeHealThreshold;
        [SerializeField] private bool isWeightStatic;
        [Space]
        [SerializeField] private bool targetAnyone;
        [SerializeField] private bool targetSelf;
        [SerializeField] private bool aoeHeal;
        [SerializeField] private bool targetSpecificCard;
        [Tooltip("Used to target the correct enemy during the fight")][SerializeField] private CardData specificCardData;

        public override IEnumerator ExecuteBehavior()
        {
            if (aoeHeal == true)
            {
                List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Enemy);

                foreach (CardMovement cardMovement in targets)
                {
                    yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                    HealGa healGa = new HealGa(healAmount, enemyCardController.cardController, cardMovement.cardController);
                    ActionSystem.instance.Perform(healGa);
                }
            }
            else
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                HealGa healGa = new HealGa(healAmount, enemyCardController.cardController, ComputeTarget());
                ActionSystem.instance.Perform(healGa);
            }
        }
        protected override CardController ComputeTarget()
        {
            if (targetSpecificCard == true && specificCardData != null)
            {
                List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Enemy);

                foreach (CardMovement cardMovement in targets)
                {
                    if (cardMovement.cardController.cardData.cardName == specificCardData.cardName)
                        return cardMovement.cardController;
                }
            }

            if (targetAnyone)
            {
                List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Enemy);

                foreach (CardMovement cardMovement in targets)
                {
                    if (cardMovement.cardController.cardHealth.currentHealth < cardMovement.cardController.cardData.hpMax * prioritizeHealThreshold)
                        return cardMovement.cardController;
                    else if (cardMovement.cardController.cardHealth.currentHealth < cardMovement.cardController.cardData.hpMax)
                        return cardMovement.cardController;
                }
            }

            if (targetSelf == true)
            {
                return enemyCardController.cardController;
            }

            return null;
        }

        public override int ComputeWeight()
        {
            if (isWeightStatic)
                return weight;

            if (targetSpecificCard == true)
            {
                CardController boss = GetSpecificCard(specificCardData);

                if (boss != null && boss.cardHealth.currentHealth == specificCardData.hpMax)
                    return 0;

                if (boss != null && boss.cardHealth.currentHealth < specificCardData.hpMax * prioritizeHealThreshold)
                    return weight * 3;
            }

            if (targetAnyone)
            {
                List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Enemy);

                int lowLifeTargets = 0;
                int damagedTargets = 0;

                foreach (CardMovement cardMovement in targets)
                {
                    if (cardMovement.cardController.cardHealth.currentHealth < cardMovement.cardController.cardData.hpMax * prioritizeHealThreshold)
                        lowLifeTargets++;
                    else if (cardMovement.cardController.cardHealth.currentHealth < cardMovement.cardController.cardData.hpMax)
                        damagedTargets++;
                }

                if (lowLifeTargets > 0)
                    return weight * 3;
                else if (damagedTargets > 0)
                    return weight;
                else
                    return 0;
            }

            if (targetSelf)
            {
                if (enemyCardController.cardController.cardHealth.currentHealth < enemyCardController.cardController.cardData.hpMax * prioritizeHealThreshold)
                    return weight * 3;
                else if (enemyCardController.cardController.cardHealth.currentHealth < enemyCardController.cardController.cardData.hpMax)
                    return weight;
                else
                    return 0;
            }

            return weight;
        }

        public override string GetDamageText()
        {
            return $"{healAmount}";
        }

        public override DamageSystem.DamageType GetDamageType()
        {
            return DamageSystem.DamageType.Heal;
        }
    }
}
