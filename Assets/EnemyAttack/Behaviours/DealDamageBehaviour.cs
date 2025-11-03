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
    public class DealDamageBehaviour : BaseEnemyBehaviour
    {
        public int damage;
        
        public override IEnumerator Execute()
        {
            Debug.Log("Deal Damage Behaviour");
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Ally);
            int randomTarget = Random.Range(0, targets.Count);
            
            DealDamageGA damageGa = new DealDamageGA(ComputeCurrentDamage(damage), enemyCardController.cardController, targets[randomTarget].cardController);
            ActionSystem.instance.Perform(damageGa);
        }
    }
}
