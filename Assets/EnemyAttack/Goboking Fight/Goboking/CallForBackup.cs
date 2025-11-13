using System;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Passives;
using Spells;
using Spells.Targeting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EnemyAttack.Goboking_Fight.Goboking
{
    public class CallForBackup : PassiveController
    {
        [SerializeField] protected List<CardData> cardsToSpawn;
        [SerializeField] private int goblinsToSpawnCount;
        [SerializeField] private int turnsBetweenSpawns;
        [SerializeField] private int maxGoblinCount;

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
            if (startTurnGa.starting != CombatLoop.CombatLoop.TurnType.Enemy)
                return;

            int currentTurn = CombatLoop.CombatLoop.instance.turnCount;
            bool isSpawnTurn = currentTurn == 1 || currentTurn % turnsBetweenSpawns == 0;
            if (isSpawnTurn && currentEnemyCount < maxGoblinCount)
            {
                int newGoblinCount = Mathf.Min(goblinsToSpawnCount, maxGoblinCount - currentEnemyCount);
                for (int i = 0; i < newGoblinCount; i++)
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
