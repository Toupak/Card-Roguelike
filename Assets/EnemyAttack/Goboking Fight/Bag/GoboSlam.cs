using Cards.Scripts;
using EnemyAttack.Behaviours;
using Spells;
using Spells.Targeting;
using System.Collections.Generic;
using UnityEngine;

public class GoboSlam : StunBehaviour
{
    [SerializeField] private int newWeightWhenAlone;

    public override int ComputeWeight()
    {
        List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Enemy);

        if (targets.Count == 2)
            return newWeightWhenAlone;

        return base.ComputeWeight();
    }
}
