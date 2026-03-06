using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.EnemyAttack;
using Combat.Spells.Targeting;
using System.Collections;
using UnityEngine;

public class ConsumeAllyBehaviour : BaseEnemyBehaviour
{
    [SerializeField] CardData cardToConsume;

    [Space]
    [SerializeField] bool isHealingAfterConsume;
    [SerializeField] int healAmount;
    
    [Space]
    [SerializeField] bool isBuffingAfterConsume;
    [SerializeField] StatusType statusType;
    [SerializeField] int stacksAmount;

    [Space]
    [SerializeField] bool isQueueNextIntentionOn;
    [SerializeField] BaseEnemyBehaviour nextIntention;

    public override IEnumerator ExecuteBehavior()
    {
        CardMovement card = TargetingSystem.instance.RetrieveCard(cardToConsume);

        if (card == null)
            yield break;

        yield return KillCard(card.cardController);

        if (isHealingAfterConsume)
        {
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            HealGa healGa = new HealGa(healAmount, enemyCardController.cardController, enemyCardController.cardController);
            ActionSystem.instance.Perform(healGa);
        }

        if (isBuffingAfterConsume)
        {
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            ApplyStatusGa statusGa = new ApplyStatusGa(statusType, stacksAmount, enemyCardController.cardController, enemyCardController.cardController);
            ActionSystem.instance.Perform(statusGa);
        }

        enemyCardController.SetNewIntention(nextIntention);

        enemyCardController.cardController.cardMovement.transform.localScale *= 3f;
    }

    private IEnumerator KillCard(CardController card)
    {
        yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

        DealDamageGA damageGa = new DealDamageGA(999, enemyCardController.cardController, card);
        ActionSystem.instance.Perform(damageGa);
    }

    public override int ComputeWeight()
    {
        CardMovement card = TargetingSystem.instance.RetrieveCard(cardToConsume);
        
        if (card == null)
            return 0;

        return base.ComputeWeight();
    }
}
