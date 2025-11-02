using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Spells;
using Spells.Targeting;
using UnityEngine;

namespace EnemyAttack.Behaviours
{
    public class StunBehaviour : BaseEnemyBehaviour
    {
        public int stunStacks;
        
        public override IEnumerator Execute()
        {
            Debug.Log("Stun Behaviour");
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Ally);
            int randomTarget = Random.Range(0, targets.Count);

            ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Stun, stunStacks, enemyCardController.cardController, targets[randomTarget].cardController);
            ActionSystem.instance.Perform(applyStatusGa);
        }
    }
}
