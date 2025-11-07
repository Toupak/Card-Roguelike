using ActionReaction;
using Cards.Scripts;
using EnemyAttack;
using Spells;
using Spells.Targeting;
using Status;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoEHeal : BaseEnemyBehaviour
{
    [SerializeField] private int healAmount;
    public override IEnumerator ExecuteBehavior()
    {
        List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Enemy);

        foreach (CardMovement cardMovement in targets)
        {
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            HealGa healGa = new HealGa(healAmount, enemyCardController.cardController, cardMovement.cardController);
            ActionSystem.instance.Perform(healGa);
        }
    }
}
