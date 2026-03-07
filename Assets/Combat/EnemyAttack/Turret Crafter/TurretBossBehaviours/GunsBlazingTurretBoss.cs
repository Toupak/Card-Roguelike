using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.EnemyAttack;
using Combat.Spells;
using Combat.Spells.Targeting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunsBlazingTurretBoss : BaseEnemyBehaviour
{
    [SerializeField] private int damage;
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
    }

    public override int ComputeWeight()
    {
        List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Enemy);

        if (targets.Count == 3)
            return weight;
        else
            return 0;
    }
}
