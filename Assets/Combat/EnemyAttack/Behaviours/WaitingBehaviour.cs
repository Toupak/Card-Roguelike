using System.Collections;
using ActionReaction;
using UnityEngine;

namespace EnemyAttack.Behaviours
{
    public class WaitingBehaviour : BaseEnemyBehaviour
    {
        public override IEnumerator ExecuteBehavior()
        {
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
        }
    }
}
