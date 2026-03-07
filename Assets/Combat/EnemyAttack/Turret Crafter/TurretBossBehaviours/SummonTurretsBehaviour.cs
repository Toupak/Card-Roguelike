using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.EnemyAttack;
using Combat.EnemyAttack.Behaviours;
using Combat.Spells;
using Combat.Spells.Targeting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonTurrets : BaseEnemyBehaviour
{
    [SerializeField] protected List<CardData> cardsToSpawn;

    public override IEnumerator ExecuteBehavior()
    {
        yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

        List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Enemy);

        for (int i = targets.Count; i < 3; i++)
            yield return SpawnRandomEnemy();
    }

    protected virtual IEnumerator SpawnRandomEnemy()
    {
        SpawnCardGA spawnCardGa = new SpawnCardGA(cardsToSpawn[Random.Range(0, cardsToSpawn.Count)], enemyCardController.cardController);
        ActionSystem.instance.Perform(spawnCardGa);

        yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
    }

    public override int ComputeWeight()
    {
        List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Enemy);

        if (targets.Count != 3)
            return weight;
        else
            return 0;
    }
}
