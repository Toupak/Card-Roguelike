using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Passives;
using Spells.Targeting;

namespace Spells.Data.Whale
{
    public class WhaleDamage : PassiveController
    {
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<StartTurnGa>(StartTurnReaction, ReactionTiming.POST);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<StartTurnGa>(StartTurnReaction, ReactionTiming.POST);
        }
        
        private void StartTurnReaction(StartTurnGa startTurnGa)
        {
            if (startTurnGa.starting == CombatLoop.CombatLoop.TurnType.Player)
            {
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.BonusDamage, 1, cardController, cardController);
                ActionSystem.instance.AddReaction(applyStatusGa);
                
                int currentSlotIndex = cardController.cardMovement.SlotIndex;

                List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(cardController.cardMovement.IsEnemyCard ? TargetType.Enemy : TargetType.Ally);
                
                for (int i = currentSlotIndex - 1; i >= 0; i--)
                {
                    if (targets[i].cardController.IsTargetable())
                    {
                        ApplyStatusGa spread = new ApplyStatusGa(applyStatusGa.type, applyStatusGa.amount, cardController, targets[i].cardController);
                        ActionSystem.instance.AddReaction(spread);
                        return;
                    }
                }
            }
        }
    }
}
