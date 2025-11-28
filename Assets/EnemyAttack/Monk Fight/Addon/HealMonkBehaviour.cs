using ActionReaction;
using Cards.Scripts;
using Spells;
using Spells.Targeting;
using System.Collections;
using System.Collections.Generic;
using CombatLoop;
using UnityEngine;

namespace EnemyAttack.Monk_Fight.Addon
{
    public class HealMonkBehaviour : BaseEnemyBehaviour
    {
        [Tooltip("Used to target the correct enemy during the fight")] [SerializeField] private CardData bossData;
        [SerializeField] private int healAmount;
        [SerializeField] private float prioritizeHealThreshold;
        [SerializeField] private bool isWeightStatic;

        protected override CardController ComputeTarget()
        {
            List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Enemy);

            foreach (CardMovement cardMovement in targets)
            {
                if (cardMovement.cardController.cardData.cardName == bossData.cardName)
                    return cardMovement.cardController;
            }

            return null;
        }

        public override IEnumerator ExecuteBehavior()
        {
            Debug.Log("Heal");
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            HealGa healGa = new HealGa(healAmount, enemyCardController.cardController, ComputeTarget());
            ActionSystem.instance.Perform(healGa);
        }

        public override int ComputeWeight()
        {
            if (isWeightStatic)
                return weight;

            CardController boss = GetSpecificCard(bossData);

            if (boss != null && boss.cardHealth.currentHealth == bossData.hpMax)
                return 0;

            if (boss != null && boss.cardHealth.currentHealth < bossData.hpMax * prioritizeHealThreshold)
                return weight * 3;

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

