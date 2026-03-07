using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.EnemyAttack;
using Combat.EnemyAttack.Behaviours;
using Combat.Spells;
using Combat.Spells.Targeting;
using Combat.Status;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTurretSelfBuff : BaseEnemyBehaviour
{
    [SerializeField] private StatusType statusType;
    [SerializeField] private int stacks;

    public override IEnumerator ExecuteBehavior()
    {
        yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
        ApplyStatusGa applyStatusGa = new ApplyStatusGa(statusType, stacks, enemyCardController.cardController, enemyCardController.cardController);
        ActionSystem.instance.Perform(applyStatusGa);
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
