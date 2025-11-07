using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using Cards.Scripts;
using CombatLoop;
using UnityEngine;

namespace EnemyAttack.Behaviours
{
    public class SpawnEnemyBehaviour : BaseEnemyBehaviour
    {
        [SerializeField] protected List<CardData> cardsToSpawn;

        public override IEnumerator ExecuteBehavior()
        {
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            yield return SpawnRandomEnemy();
        }

        protected virtual IEnumerator SpawnRandomEnemy()
        {
            EnemyHandController.instance.SpawnEnemy(cardsToSpawn[Random.Range(0, cardsToSpawn.Count)]);

            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
        }
    }
}
