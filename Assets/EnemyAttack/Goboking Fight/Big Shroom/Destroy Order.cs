using ActionReaction;
using Cards.Scripts;
using EnemyAttack;
using Spells;
using Spells.Targeting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOrder : BaseEnemyBehaviour
{
    [SerializeField] CardData cardToOrder;

    public override IEnumerator ExecuteBehavior()
    {
        yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

        List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Enemy);

        foreach (CardMovement cardMovement in targets)
        {
            //if (cardMovement.cardController != null && cardMovement.cardController.cardData.cardName == cardToOrder.cardName)
                //return cardMovement.cardController;
        }

        yield break;
        //public T Dequeue();
        //public void Enqueue(T item);
        //yield return behaviourQueue.Dequeue().ExecuteBehavior();
    }

    public override int ComputeWeight()
    {
        List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Enemy);

        if (targets.Count > 1)
            return 1;

        return weight;
    }
}
