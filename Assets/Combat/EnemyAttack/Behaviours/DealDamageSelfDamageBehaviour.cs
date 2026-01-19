using System.Collections;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.EnemyAttack.Behaviours
{
    public class DealDamageSelfDamageBehaviour : BaseEnemyBehaviour
    {
        [SerializeField] private int damage;
        [SerializeField] public int selfDamage;

        public override IEnumerator ExecuteBehavior()
        {
            Debug.Log("Deal Damage Behaviour");

            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            CardController target = ComputeTarget();
            DealDamageGA damageGa = new DealDamageGA(ComputeCurrentDamage(damage, target), enemyCardController.cardController, target);
            ActionSystem.instance.Perform(damageGa);

            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            DealDamageGA selfDamageGa = new DealDamageGA(selfDamage, enemyCardController.cardController, enemyCardController.cardController);
            ActionSystem.instance.Perform(selfDamageGa);
        }
        
        public override string GetDamageText()
        {
            return $"{ComputeCurrentDamage(damage, null)}";
        }
    }
}