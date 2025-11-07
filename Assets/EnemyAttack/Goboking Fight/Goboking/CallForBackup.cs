using System.Collections;
using ActionReaction;
using EnemyAttack.Behaviours;
using Spells;
using Spells.Targeting;
using UnityEngine;

namespace EnemyAttack.Goboking_Fight.Goboking
{
    public class CallForBackup : SpawnEnemyBehaviour
    {
        [SerializeField] private int goblinsToSpawn;
        [SerializeField] BaseEnemyBehaviour waitingBehaviourAfterSpawn;

        private int currentEnemyCount => TargetingSystem.instance.RetrieveBoard(TargetType.Enemy).Count;
        
        public override IEnumerator ExecuteBehavior()
        {
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            int spawnCount = goblinsToSpawn - (currentEnemyCount - 1);
            
            for (int i = 0;  i < spawnCount; i++)
            {
                yield return SpawnRandomEnemy();
                yield return new WaitForSeconds(0.15f);
            }

            enemyCardController.SetNewIntention(waitingBehaviourAfterSpawn, true);
        }

        public override int ComputeWeight()
        {
            if (currentEnemyCount <= 1)
            {
                if (CombatLoop.CombatLoop.instance.currentTurn != CombatLoop.CombatLoop.TurnType.Preparation)
                    enemyCardController.SetNewIntention(waitingBehaviourAfterSpawn, true);
                return weight;
            }

            return 0;
        }
    }
}
