using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.EnemyAttack;
using System.Collections;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;

public class DealDamageSelfBuffBehaviour : BaseEnemyBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private StatusType statusType;
    [SerializeField] private int statusStacks;

    public override IEnumerator ExecuteBehavior()
    {
        yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

        CardController target = ComputeTarget();

        if (target == null)
            yield break;

        DealDamageGA damageGa = new DealDamageGA(ComputeCurrentDamage(damage), enemyCardController.cardController, target);
        ActionSystem.instance.Perform(damageGa);

        yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

        ApplyStatusGa applyStatusGa = new ApplyStatusGa(statusType, statusStacks, enemyCardController.cardController, enemyCardController.cardController);
        ActionSystem.instance.Perform(applyStatusGa);
    }

    public override string GetDamageText()
    {
        return $"{ComputeCurrentDamage(damage)}";
    }
}
