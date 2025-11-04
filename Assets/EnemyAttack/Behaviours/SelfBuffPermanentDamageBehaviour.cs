using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using EnemyAttack;
using System.Collections;
using UnityEngine;

public class SelfBuffPermanentDamageBehaviour : BaseEnemyBehaviour
{
    [SerializeField] private int damageStacks;

    public override IEnumerator ExecuteBehavior()
    {
        Debug.Log("SelfBuffPermanentDamage");
        yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

        ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.PermanentBonusDamage, damageStacks, enemyCardController.cardController, enemyCardController.cardController);
        ActionSystem.instance.Perform(applyStatusGa);
    }
}
