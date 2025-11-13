using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Spells;
using Spells.Targeting;
using Status;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyAttack.Behaviours
{
    public class DealDamageSelfDamageBehaviour : BaseEnemyBehaviour
    {
        [SerializeField] private int damage;
        [SerializeField] public int selfDamage;

        public override IEnumerator ExecuteBehavior()
        {
            Debug.Log("Deal Damage Behaviour");

            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            DealDamageGA damageGa = new DealDamageGA(ComputeCurrentDamage(damage), enemyCardController.cardController, ComputeTarget());
            ActionSystem.instance.Perform(damageGa);

            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            DealDamageGA selfDamageGa = new DealDamageGA(selfDamage, enemyCardController.cardController, enemyCardController.cardController);
            ActionSystem.instance.Perform(selfDamageGa);
        }
        
        public override string GetDamageText()
        {
            return $"{ComputeCurrentDamage(damage)}";
        }
    }
}