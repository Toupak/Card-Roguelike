using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using Cards.Scripts;
using Spells;
using Spells.Targeting;
using UnityEngine;

namespace EnemyAttack.Monk_Fight.Addon
{
    public class HealMonkBehaviour : BaseEnemyBehaviour
    {
        [Tooltip("Used to target the correct enemy during the fight")] [SerializeField] private CardData bossData;
        [SerializeField] private int healAmount;

        protected override CardController ComputeTarget(bool canBeTaunted = false)
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
    }
}

