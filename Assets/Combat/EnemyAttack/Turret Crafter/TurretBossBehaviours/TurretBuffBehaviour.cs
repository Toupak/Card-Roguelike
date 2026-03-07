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
using System.Linq;
using UnityEngine;

public class TurretBuffBehaviour : BaseEnemyBehaviour
{
    [SerializeField] private StatusType statusType;
    [SerializeField] private int stacks;
    [SerializeField] private List<CardData> turrets;

    public override IEnumerator ExecuteBehavior()
    {
        List<CardMovement> targets = ComputeTargetList(true);

        foreach(CardMovement target in targets)
        {
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            ApplyStatusGa applyStatusGa = new ApplyStatusGa(statusType, stacks, enemyCardController.cardController, target.cardController);
            ActionSystem.instance.Perform(applyStatusGa);
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
