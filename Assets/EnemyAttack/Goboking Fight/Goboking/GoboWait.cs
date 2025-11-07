using System.Collections;
using ActionReaction;
using Spells;
using Spells.Targeting;
using UnityEngine;

namespace EnemyAttack.Goboking_Fight.Goboking
{
    public class GoboWait : BaseEnemyBehaviour
    {
        private int currentEnemyCount => TargetingSystem.instance.RetrieveBoard(TargetType.Enemy).Count;
        
        public override IEnumerator ExecuteBehavior()
        {
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
        }
        
        public override int ComputeWeight()
        {
            if (currentEnemyCount <= 1)
                return 0;

            return weight;
        }
    }
}