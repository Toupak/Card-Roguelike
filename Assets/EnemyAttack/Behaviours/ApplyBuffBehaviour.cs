using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Spells;
using Spells.Targeting;
using Status;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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

        protected override CardController ComputeTarget(bool canBeTaunted = false)
        {
            if (!isTargetEnemy)
            {
                List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Ally);

                if (targets.Count < 1)
                    return null;

                if (canBeTaunted)
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
            else
            {
                List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Enemy);

                if (targets.Count < 1)
                    return null;

                if (cantTargetHimself)
                {
                    List<CardController> otherCards = new ();

                    foreach (CardMovement cardMovement in targets)
                    {
                        if (cardMovement.cardController != null && cardMovement.cardController.cardData.cardName != enemyCardController.cardData.cardName)
                            otherCards.Add(cardMovement.cardController);
                    }

                    if (otherCards.Count > 0)
                    {
                        int randomTarget = Random.Range(0, otherCards.Count);

                        return otherCards[randomTarget];
                    }
                    else
                        return null;
                }
                else
                {
                    int randomTarget = Random.Range(0, targets.Count);

                    return targets[randomTarget].cardController;
                }
            }
        }
    }
}
