using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Status;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyAttack.Behaviours
{
    public class ApplyBuffBehaviour : BaseEnemyBehaviour
    {
        [SerializeField] private StatusType statusType;
        [SerializeField] private int stacks;
        [SerializeField] private int hitCount;
        [SerializeField] private bool hitSameTarget;
        [SerializeField] private bool isTargetEnemy;
        [SerializeField] private bool cantTargetHimself;
        [SerializeField] private bool targetSpecificCard;
        [SerializeField] private bool targetAllPlayerCards;
        [SerializeField] private CardData specificCard;

        public override IEnumerator ExecuteBehavior()
        {
            Debug.Log("Apply Buff Behaviour");

            if (hitSameTarget)
            {
                CardController target = ComputeTarget();
                for (int i = 0; i < hitCount; i++)
                {
                    yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                    ApplyStatusGa applyStatusGa = new ApplyStatusGa(statusType, stacks, enemyCardController.cardController, target);
                    ActionSystem.instance.Perform(applyStatusGa);
                }
            }
            else if (targetAllPlayerCards)
            {
                List<CardMovement> targets = ComputeTargetList(isTargetEnemy);
                
                foreach (CardMovement target in targets)
                {
                    yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                    ApplyStatusGa applyStatusGa = new ApplyStatusGa(statusType, stacks, enemyCardController.cardController, target.cardController);
                    ActionSystem.instance.Perform(applyStatusGa);
                }
            }
            else
            {
                for (int i = 0; i < hitCount; i++)
                {
                    yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                    ApplyStatusGa applyStatusGa = new ApplyStatusGa(statusType, stacks, enemyCardController.cardController, ComputeTarget());
                    ActionSystem.instance.Perform(applyStatusGa);
                }
            }
        }

        protected override CardController ComputeTarget()
        {
            List<CardMovement> targets = ComputeTargetList(isTargetEnemy, !cantTargetHimself);

            if (targets.Count < 1)
                return null;

            if (targetSpecificCard)
            {
                return ComputeSpecificTarget(targets, specificCard);
            }

            if (!isTargetEnemy)
            {
                foreach (CardMovement cardMovement in targets)
                {
                    if (StatusSystem.instance.IsCardAfflictedByStatus(cardMovement.cardController, StatusType.Taunt))
                        return cardMovement.cardController;
                }
            }
            
            int randomTarget = Random.Range(0, targets.Count);
            return targets[randomTarget].cardController;
        }

        protected CardController ComputeSpecificTarget(List<CardMovement> targets, CardData specificTarget)
        {
            foreach (CardMovement cardMovement in targets)
            {
                if (cardMovement.cardController.cardData.cardName == specificTarget.cardName)
                    return cardMovement.cardController;
            }

            return null;
        }
    }
}
