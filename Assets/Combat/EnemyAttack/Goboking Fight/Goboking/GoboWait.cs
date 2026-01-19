using System.Collections;
using ActionReaction;
using UnityEngine;

namespace EnemyAttack.Goboking_Fight.Goboking
{
    public class GoboWait : BaseEnemyBehaviour
    {
        public override IEnumerator ExecuteBehavior()
        {
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
        }
        
        public override int ComputeWeight()
        {
            return weight;
        }
    }
}