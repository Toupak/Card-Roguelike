using ActionReaction;
using ActionReaction.Game_Actions;
using EnemyAttack;
using System.Collections;
using UnityEngine;

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
            DealDamageGA damageGa = new DealDamageGA(ComputeCurrentDamage(damage), enemyCardController.cardController, ComputeTarget(true));
            ActionSystem.instance.Perform(damageGa);
        }
    }
}
