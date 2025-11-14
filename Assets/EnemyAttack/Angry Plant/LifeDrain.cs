using ActionReaction;
using EnemyAttack.Behaviours;
using System.Collections;
using UnityEngine;

public class LifeDrain : DealDamageBehaviour
{
    [SerializeField] private int healAmount;

    public override IEnumerator ExecuteBehavior()
    {
        yield return base.ExecuteBehavior();

        yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

        HealGa healGa = new HealGa(healAmount, enemyCardController.cardController, enemyCardController.cardController);
        ActionSystem.instance.Perform(healGa);
    }
}
