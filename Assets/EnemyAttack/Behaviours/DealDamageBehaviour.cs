using System.Collections;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace EnemyAttack.Behaviours
{
    public class DealDamageBehaviour : BaseEnemyBehaviour
    {
        public int damage;
        
        public override IEnumerator ExecuteBehavior()
        {
            Debug.Log("Deal Damage Behaviour");
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            CardController target = ComputeTarget();

            if (target == null)
                yield break;
            
            DealDamageGA damageGa = new DealDamageGA(ComputeCurrentDamage(damage), enemyCardController.cardController, target);
            ActionSystem.instance.Perform(damageGa);
        }
        
        public override string GetDamageText()
        {
            return $"{ComputeCurrentDamage(damage)}";
        }
    }
}
