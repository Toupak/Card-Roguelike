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

    [SerializeField] BaseEnemyBehaviour waitingBehaviour;
    [SerializeField] BaseEnemyBehaviour suicideBehaviour;

    private List<CardController> currentCardsToOrder = new ();

    public override IEnumerator ExecuteBehavior()
    {
        yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

        List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Enemy);

        currentCardsToOrder.Clear();

        foreach (CardMovement cardMovement in targets)
        {
            if (cardMovement.cardController != null && cardMovement.cardController.cardData.cardName == cardToOrder.cardName)
                currentCardsToOrder.Add(cardMovement.cardController);        
        }

        if (currentCardsToOrder.Count == 0)
            yield break;

        int random = Random.Range(0, currentCardsToOrder.Count);

        currentCardsToOrder[random].enemyCardController.SetNewIntention(waitingBehaviour);
        currentCardsToOrder[random].enemyCardController.SetNewIntention(suicideBehaviour);
    }

    public override int ComputeWeight()
    {
        List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Enemy);

        if (targets.Count > 1)
            return 1;

        return weight;
    }
}
