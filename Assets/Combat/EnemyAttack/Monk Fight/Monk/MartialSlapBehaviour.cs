using System.Collections;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.EnemyAttack.Monk_Fight.Monk
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
                CardController target = ComputeTarget();
                DealDamageGA damageGa = new DealDamageGA(ComputeCurrentDamage(damage, target), enemyCardController.cardController, target);
                ActionSystem.instance.Perform(damageGa);
            }
        }
        
        public override string GetDamageText()
        {
            return $"{ComputeCurrentDamage(damage, null)}X{hitCount}";
        }
    }
}
