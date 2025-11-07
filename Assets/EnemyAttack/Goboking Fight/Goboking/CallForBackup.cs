using ActionReaction;
using Cards.Scripts;
using CombatLoop;
using EnemyAttack;
using Spells;
using Spells.Data.Thorse;
using Spells.Targeting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallForBackup : SpawnEnemyBehaviour
{
    [SerializeField] private int goblinsToSpawn;
    [SerializeField] BaseEnemyBehaviour waitingBehaviourAfterSpawn;

    public override IEnumerator ExecuteBehavior()
    {
        yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

        List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Enemy);

        for (int i = 0;  i < goblinsToSpawn; i++)
        {
            EnemyHandController.instance.SpawnEnemy(cardToSpawn);
            yield return new WaitForSeconds(0.15f);
        }

        enemyCardController.SetNewIntention(waitingBehaviourAfterSpawn, true);
    }

    public override int ComputeWeight()
    {
        List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Enemy);

        if (targets.Count == 1)
            return 1;

        return weight;
    }
}
