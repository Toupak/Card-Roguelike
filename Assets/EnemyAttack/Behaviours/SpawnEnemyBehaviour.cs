using ActionReaction;
using Cards.Scripts;
using CombatLoop;
using EnemyAttack;
using Spells;
using Spells.Targeting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyBehaviour : BaseEnemyBehaviour
{
    [SerializeField] protected CardData cardToSpawn;

    public override IEnumerator ExecuteBehavior()
    {
        yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

        EnemyHandController.instance.SpawnEnemy(cardToSpawn);
    }
}
