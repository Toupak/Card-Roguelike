using Cards.Scripts;
using EnemyAttack.Behaviours;
using Spells;
using Spells.Targeting;
using System.Collections.Generic;
using UnityEngine;

public class PepTalk : ApplyBuffBehaviour
{
    public override int ComputeWeight()
    {
        List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Enemy);

        if (targets.Count >= 4)
            return weight;
        else if (targets.Count == 3)
            return weight * 2;
        else if (targets.Count == 2)
            return weight * 3;
        else
            return 0;
    }
}
