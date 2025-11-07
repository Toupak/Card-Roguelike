using System.Collections;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace EnemyAttack.Behaviours
{
    public class ApplyBuffBehaviour : BaseEnemyBehaviour
    {
        [SerializeField] private StatusType statusType;
        [SerializeField] private int stacks;
        [SerializeField] private int hitCount;
        [SerializeField] private bool hitSameTarget;

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
    }
}
