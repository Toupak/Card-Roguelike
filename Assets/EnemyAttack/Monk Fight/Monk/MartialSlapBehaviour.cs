using System.Collections;
using ActionReaction;
using ActionReaction.Game_Actions;
using CombatLoop;
using UnityEngine;

namespace EnemyAttack.Monk_Fight.Monk
{
    public class MartialSlapBehaviour : BaseEnemyBehaviour
    {
        [SerializeField] private int damage;
        [SerializeField] private uint hitCount;

        public override IEnumerator ExecuteBehavior()
        {
            Debug.Log("Deal Damage Behaviour");

            for (int i = 0;  i < hitCount; i++)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                DealDamageGA damageGa = new DealDamageGA(ComputeCurrentDamage(damage), enemyCardController.cardController, ComputeTarget());
                ActionSystem.instance.Perform(damageGa);
            }
        }
        
        public override string GetDamageText()
        {
            return $"{ComputeCurrentDamage(damage)}X{hitCount}";
        }
    }
}
