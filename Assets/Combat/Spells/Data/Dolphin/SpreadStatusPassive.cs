using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Passives;
using Spells.Targeting;
using Status;
using Status.Data;
using UnityEngine;

namespace Spells.Data.Dolphin
{
    public class SpreadStatusPassive : PassiveController
    {
        public enum SpreadDirection
        {
            Left,
            Right,
            None
        }
        
        [SerializeField] private bool spreadLeft;
        [SerializeField] private bool spreadRight;
        
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<ApplyStatusGa>(ApplyStatusReaction, ReactionTiming.POST);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<ApplyStatusGa>(ApplyStatusReaction, ReactionTiming.POST);
        }

        private void ApplyStatusReaction(ApplyStatusGa applyStatusGa)
        {
            if (applyStatusGa.target == cardController && applyStatusGa.type != StatusType.Dive && StatusSystem.instance.GetStatusData(applyStatusGa.type).effectType != StatusData.EffectType.Negative)
            {
                int currentSlotIndex = cardController.cardMovement.SlotIndex;

                List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(cardController.cardMovement.IsEnemyCard ? TargetType.Enemy : TargetType.Ally, true);

                if (spreadLeft && applyStatusGa.spreadDirection != SpreadDirection.Right)
                {
                    for (int i = currentSlotIndex - 1; i >= 0; i--)
                    {
                        if (targets[i].cardController.IsTargetable())
                        {
                            SpreadStatus(targets[i].cardController, applyStatusGa, SpreadDirection.Left);
                            break;
                        }
                    }
                }

                if (spreadRight && applyStatusGa.spreadDirection != SpreadDirection.Left)
                {
                    for (int i = currentSlotIndex + 1; i < targets.Count; i++)
                    {
                        if (targets[i].cardController.IsTargetable())
                        {
                            SpreadStatus(targets[i].cardController, applyStatusGa, SpreadDirection.Right);
                            break;
                        }
                    }
                }
            }
        }

        private void SpreadStatus(CardController target, ApplyStatusGa applyStatusGa, SpreadDirection direction)
        {
            ApplyStatusGa spread = new ApplyStatusGa(applyStatusGa.type, applyStatusGa.amount, cardController, target);
            spread.spreadDirection = direction;
            ActionSystem.instance.AddReaction(spread);
        }
    }
}
