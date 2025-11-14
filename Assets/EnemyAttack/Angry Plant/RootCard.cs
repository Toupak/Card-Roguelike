using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using CombatLoop;
using EnemyAttack;
using System.Collections;
using UnityEngine;

public class RootCard : BaseEnemyBehaviour
{
    [SerializeField] private int stacks;

    public override IEnumerator ExecuteBehavior()
    {
        yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

        CardController target = ComputeTarget();

        target.cardMovement.CurrentSlot.board.SendCardToOtherBoard(target.cardMovement.SlotIndex, EnemyHandController.instance.container);

        yield return new WaitForSeconds(0.5f);

        ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Captured, stacks, enemyCardController.cardController, target);
        ActionSystem.instance.Perform(applyStatusGa);

        //Deal damage to card every turn
    }
}
