using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using EnemyAttack;
using Spells;
using Spells.Targeting;
using Status;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealMonkBehaviour : BaseEnemyBehaviour
{
    [SerializeField] private int healAmount;

    protected override CardController ComputeTarget(bool canBeTaunted = false)
    {
        List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Enemy);

        foreach (CardMovement cardMovement in targets)
        {
            if (cardMovement.cardController.cardData.cardName == "CrimsonMonk")
                return cardMovement.cardController;
        }

        return null;
    }

    public override IEnumerator ExecuteBehavior()
    {
        Debug.Log("Heal");
        yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

        HealGa healGa = new HealGa(healAmount, enemyCardController.cardController, ComputeTarget());
        ActionSystem.instance.Perform(healGa);
    }
}
