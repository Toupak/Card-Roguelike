using Cards.Scripts;
using EnemyAttack;
using Spells;
using Spells.Targeting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallforBackup : BaseEnemyBehaviour
{
    public override IEnumerator ExecuteBehavior()
    {
        throw new System.NotImplementedException();
    }

    public override int ComputeWeight()
    {
        List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Enemy);

        if (targets.Count == 1)
            return 1;

        return weight;
    }
}
