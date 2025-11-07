using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using EnemyAttack;
using EnemyAttack.Behaviours;
using NUnit.Framework;
using Spells;
using Spells.Targeting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class KingStomp : BaseEnemyBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private int buffStacks;

    public override IEnumerator ExecuteBehavior()
    {
        yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

        List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Ally);

        foreach(CardMovement target in targets)
        {
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            DealDamageGA dealDamage = new DealDamageGA(ComputeCurrentDamage(damage), enemyCardController.cardController, target.cardController);
            ActionSystem.instance.Perform(dealDamage);
        }

        yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

        ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.PermanentBonusDamage, buffStacks, enemyCardController.cardController, enemyCardController.cardController);
        ActionSystem.instance.Perform(applyStatusGa);
    }
}
