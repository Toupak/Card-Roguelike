using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using EnemyAttack;
using EnemyAttack.Behaviours;
using System.Collections;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;

public class DamageDebuffBehaviour : BaseEnemyBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private StatusType statusType;
    [SerializeField] private int stacks;

    public override IEnumerator ExecuteBehavior()
    {
        yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

        CardController target = ComputeTarget();

        if (target == null)
            yield break;

        DealDamageGA damageGa = new DealDamageGA(ComputeCurrentDamage(damage, target), enemyCardController.cardController, target);
        ActionSystem.instance.Perform(damageGa);

        yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

        ApplyStatusGa applyStatusGa = new ApplyStatusGa(statusType, stacks, enemyCardController.cardController, target);
        ActionSystem.instance.Perform(applyStatusGa);
    }
    public override string GetDamageText()
    {
        return $"{ComputeCurrentDamage(damage, null)}";
    }
}
