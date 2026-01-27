using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;
using Combat.Spells.Targeting;
using UnityEngine;

namespace Combat.Spells.Data.Gardener.Passives.Blue
{
    public class GardenerPlantPassiveBlue : PassiveController
    {
        [SerializeField] private StatusType status;
        [SerializeField] private bool allTargets;
        
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
            if (endTurnGa.ending == CombatLoop.TurnType.Player)
            {
                if (allTargets)
                    ApplyStatusToAllTargets();
                else
                    ApplyStatusToRandomTarget();
            }
        }

        private void ApplyStatusToAllTargets()
        {
            List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Enemy);

            foreach (CardMovement target in targets)
            {
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(status, 1, cardController, target.cardController);
                ActionSystem.instance.AddReaction(applyStatusGa);
            }
        }

        private void ApplyStatusToRandomTarget()
        {
            CardMovement target = TargetingSystem.instance.RetrieveRandomCard(TargetType.Enemy);
            ApplyStatusGa applyStatusGa = new ApplyStatusGa(status, 1, cardController, target.cardController);
            ActionSystem.instance.AddReaction(applyStatusGa);
        }
    }
}
