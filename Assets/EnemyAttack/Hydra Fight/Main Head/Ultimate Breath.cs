using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using CombatLoop;
using Spells;
using Spells.Targeting;
using UnityEngine;

namespace EnemyAttack.Hydra_Fight.Main_Head
{
    public class UltimateBreath : BaseEnemyBehaviour
    {
        [SerializeField] protected int damage;
        [SerializeField] protected BaseEnemyBehaviour waiting;

        public override IEnumerator ExecuteBehavior()
        {
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Ally);

            foreach (CardMovement target in targets)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                DealDamageGA dealDamage = new DealDamageGA(ComputeCurrentDamage(damage), enemyCardController.cardController, target.cardController);
                ActionSystem.instance.Perform(dealDamage);
            }

            enemyCardController.SetNewIntention(waiting);
        }
    
        public override string GetDamageText()
        {
            return $"{damage}";
        }

        public override DamageSystem.DamageType GetDamageType()
        {
            return DamageSystem.DamageType.Crit;
        }
    }
}
