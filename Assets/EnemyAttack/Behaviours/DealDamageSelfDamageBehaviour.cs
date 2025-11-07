using ActionReaction;
using ActionReaction.Game_Actions;
using EnemyAttack;
using System.Collections;
using UnityEngine;

public class DealDamageSelfDamageBehaviour : BaseEnemyBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] public int selfDamage;
    
    public override IEnumerator ExecuteBehavior()
    {
        Debug.Log("Deal Damage Behaviour");

        yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

        DealDamageGA damageGa = new DealDamageGA(ComputeCurrentDamage(damage), enemyCardController.cardController, ComputeTarget(true));
        ActionSystem.instance.Perform(damageGa);

        yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

        DealDamageGA selfDamageGa = new DealDamageGA(selfDamage, enemyCardController.cardController, enemyCardController.cardController);
        ActionSystem.instance.Perform(selfDamageGa);
    }
}
