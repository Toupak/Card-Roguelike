using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Passives;
using Spells.Targeting;

namespace Spells.Data.Whale
{
    public class WhaleArmor : PassiveController
    {
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<EndTurnGA>(EndTurnReaction, ReactionTiming.PRE);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<EndTurnGA>(EndTurnReaction, ReactionTiming.PRE);
        }
        
        private void EndTurnReaction(EndTurnGA endTurnGa)
        {
            if (endTurnGa.ending == CombatLoop.CombatLoop.TurnType.Player)
            {
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Armor, 1, cardController, cardController);
                ActionSystem.instance.AddReaction(applyStatusGa);
                
                int currentSlotIndex = cardController.cardMovement.SlotIndex;

                List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(cardController.cardMovement.IsEnemyCard ? TargetType.Enemy : TargetType.Ally, true);
                
                for (int i = currentSlotIndex + 1; i < targets.Count; i++)
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
