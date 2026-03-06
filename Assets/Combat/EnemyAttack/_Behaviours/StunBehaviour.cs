using System.Collections;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.EnemyAttack.Behaviours
{
    public class StunBehaviour : BaseEnemyBehaviour
    {
        public int stunStacks;
        
        public override IEnumerator ExecuteBehavior()
        {
            Debug.Log("Stun Behaviour");
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Stun, stunStacks, enemyCardController.cardController, ComputeTarget());
            ActionSystem.instance.Perform(applyStatusGa);
        }
    }
}
