using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace EnemyAttack.Behaviours
{
    public class SpawnEnemyBehaviour : BaseEnemyBehaviour
    {
        [SerializeField] protected List<CardData> cardsToSpawn;
        [SerializeField] private int spawnCount;

        public override IEnumerator ExecuteBehavior()
        {
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            for (int i = 0; i < Mathf.Max(1, spawnCount); i++)
                yield return SpawnRandomEnemy();
        }

        protected virtual IEnumerator SpawnRandomEnemy()
        {
            SpawnCardGA spawnCardGa = new SpawnCardGA(cardsToSpawn[Random.Range(0, cardsToSpawn.Count)], enemyCardController.cardController);
            ActionSystem.instance.Perform(spawnCardGa);
            
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
        }
    }
}
