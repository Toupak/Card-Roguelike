using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using Cards.Scripts;
using UnityEngine;

namespace Combat.EnemyAttack.Angry_Plant
{
    public class BurpCard : BaseEnemyBehaviour
    {
        [HideInInspector] public CardMovement cardRooted;

        public override IEnumerator ExecuteBehavior()
        {
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            List<CardMovement> targets = ComputeTargetList(true, false);

            foreach(CardMovement target in targets)
            {
                if (target.cardController.cardStatus.IsStatusApplied(StatusType.Captured))
                {
                    target.CurrentSlot.board.SendCardToOtherBoard(target.SlotIndex, CombatLoop.instance.playerBoard);

                    yield return new WaitForSeconds(0.5f);
                    yield break;
                }
            }
        }
    }
}
