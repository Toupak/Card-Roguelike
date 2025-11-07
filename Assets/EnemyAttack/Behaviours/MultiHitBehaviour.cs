using System.Collections;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace EnemyAttack.Behaviours
{
    public class MultiHitBehaviour : BaseEnemyBehaviour
    {
        [SerializeField] private int damage;
        [SerializeField] private int hitCount;
        [SerializeField] private bool hitSameTarget;

        public override IEnumerator ExecuteBehavior()
        {
            Debug.Log("Deal Damage Behaviour");

            if (hitSameTarget)
            {
                CardController target = ComputeTarget(true);
                for (int i = 0; i < hitCount; i++)
                {
                    yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                    DealDamageGA damageGa = new DealDamageGA(ComputeCurrentDamage(damage), enemyCardController.cardController, target);
                    ActionSystem.instance.Perform(damageGa);
                }
            }
            else
            {
                for (int i = 0; i < hitCount; i++)
                {
                    yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                    DealDamageGA damageGa = new DealDamageGA(ComputeCurrentDamage(damage), enemyCardController.cardController, ComputeTarget(true));
                    ActionSystem.instance.Perform(damageGa);
                }
            }
        }
    }
}
