using ActionReaction;
using EnemyAttack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFixedIntentions : BaseEnemyBehaviour
{
    [SerializeField] List<BaseEnemyBehaviour> intentions = new();

    public override IEnumerator ExecuteBehavior()
    {
        yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

        for (int count= 0; count < 50; count++)
        {
            for (int i = 0; i < intentions.Count; i++)
            {
                enemyCardController.SetNewIntention(intentions[i]);
            }
        }
    }
}
