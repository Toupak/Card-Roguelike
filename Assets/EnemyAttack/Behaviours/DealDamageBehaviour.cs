using System.Collections;
using ActionReaction;
using ActionReaction.Game_Actions;
using UnityEngine;

namespace EnemyAttack.Behaviours
{
    public class DealDamageBehaviour : BaseEnemyBehaviour
    {
        public int damage;
        
        public override IEnumerator Execute()
        {
            Debug.Log("Deal Damage Behaviour");
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            
            DealDamageGA damageGa = new DealDamageGA(ComputeCurrentDamage(damage), enemyCardController.cardController, ComputeTarget(true));
            ActionSystem.instance.Perform(damageGa);
        }
    }
}
