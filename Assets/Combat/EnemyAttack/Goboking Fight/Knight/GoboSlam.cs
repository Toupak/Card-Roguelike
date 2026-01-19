using System.Collections.Generic;
using Cards.Scripts;
using Combat.EnemyAttack.Behaviours;
using Combat.Spells;
using Combat.Spells.Targeting;
using UnityEngine;

namespace Combat.EnemyAttack.Goboking_Fight.Knight
{
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
}
