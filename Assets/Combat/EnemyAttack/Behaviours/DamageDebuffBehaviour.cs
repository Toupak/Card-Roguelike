using System.Collections;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.EnemyAttack.Behaviours
{
    public class DamageDebuffBehaviour : BaseEnemyBehaviour
    {
        [SerializeField] private int damage;
        [SerializeField] private StatusType statusType;
        [SerializeField] private int stacks;
        [SerializeField] private int hitCount;

        public override IEnumerator ExecuteBehavior()
        {
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            if (hitCount <= 0) //to hit at least once if hitCount isn't set
                hitCount = 1;

            for (int i = 0; i < hitCount; i++)
            {
                CardController target = ComputeTarget();

                if (target == null)
                    yield break;

                DealDamageGA damageGa = new DealDamageGA(ComputeCurrentDamage(damage, target), enemyCardController.cardController, target);
                ActionSystem.instance.Perform(damageGa);

                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                ApplyStatusGa applyStatusGa = new ApplyStatusGa(statusType, stacks, enemyCardController.cardController, target);
                ActionSystem.instance.Perform(applyStatusGa);
            }
        }

        public override string GetDamageText()
        {
            return $"{ComputeCurrentDamage(damage, null)}";
        }
    }
}
