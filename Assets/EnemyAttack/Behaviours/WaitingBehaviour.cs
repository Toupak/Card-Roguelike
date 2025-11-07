using ActionReaction;
using EnemyAttack;
using System.Collections;
using UnityEngine;

public class WaitingBehaviour : BaseEnemyBehaviour
{
    public override IEnumerator ExecuteBehavior()
    {
        yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
    }
}
