using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Spells;
using Combat.Spells.Targeting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Combat.Passives.Common_Passives.Spawn_Enemy
{
    public class CallForBackupPassive : PassiveController
    {
        [SerializeField] protected List<CardData> cardsToSpawn;
        [SerializeField] private int cardToSpawnCount;
        [SerializeField] private int turnsBetweenSpawns;
        [SerializeField] private int maxEnemyCount;

        private int currentEnemyCount => TargetingSystem.instance.RetrieveBoard(TargetType.Enemy).Count;

        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<StartTurnGa>(StartTurnReaction, ReactionTiming.POST);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<StartTurnGa>(StartTurnReaction, ReactionTiming.POST);
        }

        private void StartTurnReaction(StartTurnGa startTurnGa)
        {
            if (startTurnGa.starting != CombatLoop.TurnType.Enemy)
                return;

            int currentTurn = CombatLoop.instance.turnCount;
            bool isSpawnTurn = currentTurn == 1 || currentTurn % turnsBetweenSpawns == 0;
            if (isSpawnTurn && currentEnemyCount < maxEnemyCount)
            {
                int newEnemyCount = Mathf.Min(cardToSpawnCount, maxEnemyCount - currentEnemyCount);
                for (int i = 0; i < newEnemyCount; i++)
                {
                    SpawnRandomEnemy();
                }
            }
        }

        private void SpawnRandomEnemy()
        {
            SpawnCardGA spawnCardGa = new SpawnCardGA(cardsToSpawn[Random.Range(0, cardsToSpawn.Count)], cardController);
            ActionSystem.instance.AddReaction(spawnCardGa);
        }
    }
}
