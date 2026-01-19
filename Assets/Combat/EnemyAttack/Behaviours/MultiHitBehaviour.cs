using System.Collections;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.EnemyAttack.Behaviours
{
    public class MultiHitBehaviour : BaseEnemyBehaviour
    {
        [SerializeField] private int damage;
        [SerializeField] private int hitCount;
        [SerializeField] private bool hitSameTarget;

        public override IEnumerator ExecuteBehavior()
        {
            Debug.Log("Multi Hit Behaviour");

            if (hitSameTarget)
            {
                CardController target = ComputeTarget();
                for (int i = 0; i < hitCount; i++)
                {
                    yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                    DealDamageGA damageGa = new DealDamageGA(ComputeCurrentDamage(damage, target), enemyCardController.cardController, target);
                    ActionSystem.instance.Perform(damageGa);
                }
            }
            else
            {
                for (int i = 0; i < hitCount; i++)
                {
                    yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                    CardController target = ComputeTarget();
                    DealDamageGA damageGa = new DealDamageGA(ComputeCurrentDamage(damage, target), enemyCardController.cardController, target);
                    ActionSystem.instance.Perform(damageGa);
                }
            }
        }
        
        public override string GetDamageText()
        {
            return $"{ComputeCurrentDamage(damage, null)}X{hitCount}";
        }
    }
}
