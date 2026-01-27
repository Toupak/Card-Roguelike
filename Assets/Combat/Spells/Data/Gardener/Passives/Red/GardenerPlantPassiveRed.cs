using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using BoomLib.Tools;
using Cards.Scripts;
using Combat.Passives;
using Combat.Spells.Targeting;
using UnityEngine;

namespace Combat.Spells.Data.Gardener.Passives.Red
{
    public class GardenerPlantPassiveRed : PassiveController
    {
        [SerializeField] private int damage;
        [SerializeField] private int targetCount;
        
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
                if (targetCount > 1)
                    AttackTargets();
                else
                    AttackTarget();
            }
        }

        private void AttackTargets()
        {
            List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Enemy);
            targets.Shuffle();

            for (int i = 0; i < targetCount && i < targets.Count; i++)
            {
                DealDamageGA dealDamageGa = new DealDamageGA(damage, cardController, targets[i].cardController);
                ActionSystem.instance.AddReaction(dealDamageGa);
            }
        }

        private void AttackTarget()
        {
            CardMovement target = TargetingSystem.instance.RetrieveRandomCard(TargetType.Enemy);
            DealDamageGA dealDamageGa = new DealDamageGA(damage, cardController, target.cardController);
            ActionSystem.instance.AddReaction(dealDamageGa);
        }
    }
}
