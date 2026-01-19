using System.Collections.Generic;
using System.Linq;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;
using Combat.Spells.Targeting;
using Random = UnityEngine.Random;

namespace Combat.Spells.Data.Faces.Tokens
{
    public class FaceTokenAttackPassive : PassiveController
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
            if (endTurnGa.ending == CombatLoop.TurnType.Player)
            {
                List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Enemy).Where((c) => c.cardController.cardStatus.IsStatusApplied(StatusType.Terror)).ToList();
                
                if (targets.Count < 1)
                    return;

                DealDamageGA dealDamageGa = new DealDamageGA(cardController.ComputeCurrentDamage(1), cardController, targets[Random.Range(0, targets.Count)].cardController);
                ActionSystem.instance.AddReaction(dealDamageGa);
            }
        }
    }
}
