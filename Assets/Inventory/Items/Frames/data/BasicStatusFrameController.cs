using System;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat;
using Combat.Spells;
using Combat.Spells.Targeting;
using UnityEngine;

namespace Inventory.Items.Frames.data
{
    public class BasicStatusFrameController : FrameController
    {
        [SerializeField] private StatusType statusType;
        [SerializeField] private StatusBehaviour behaviour;
        [SerializeField] private StatusBehaviourTimings timing;
        [SerializeField] private StatusBehaviourTarget target;
        
        private void OnEnable()
        {
            switch (timing)
            {
                case StatusBehaviourTimings.OnTurnStart:
                    ActionSystem.SubscribeReaction<StartTurnGa>(StartTurnReaction, ReactionTiming.POST);
                    break;
                case StatusBehaviourTimings.OnTurnEnd:
                    ActionSystem.SubscribeReaction<EndTurnGA>(EndTurnReaction, ReactionTiming.POST);
                    break;
                case StatusBehaviourTimings.OnDamageDealt:
                    ActionSystem.SubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.POST);
                    break;
                case StatusBehaviourTimings.OnDamageReceived:
                    ActionSystem.SubscribeReaction<DealDamageGA>(ReceiveDamageReaction, ReactionTiming.POST);
                    break;
                case StatusBehaviourTimings.OnCombatStart:
                    ActionSystem.SubscribeReaction<StartTurnGa>(StartCombatReaction, ReactionTiming.POST);
                    break;
                case StatusBehaviourTimings.OnCombatEnd:
                case StatusBehaviourTimings.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnDisable()
        {
            switch (timing)
            {
                case StatusBehaviourTimings.OnTurnStart:
                    ActionSystem.UnsubscribeReaction<StartTurnGa>(StartTurnReaction, ReactionTiming.POST);
                    break;
                case StatusBehaviourTimings.OnTurnEnd:
                    ActionSystem.UnsubscribeReaction<EndTurnGA>(EndTurnReaction, ReactionTiming.POST);
                    break;
                case StatusBehaviourTimings.OnDamageDealt:
                    ActionSystem.UnsubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.POST);
                    break;
                case StatusBehaviourTimings.OnDamageReceived:
                    ActionSystem.UnsubscribeReaction<DealDamageGA>(ReceiveDamageReaction, ReactionTiming.POST);
                    break;
                case StatusBehaviourTimings.OnCombatStart:
                    ActionSystem.UnsubscribeReaction<StartTurnGa>(StartCombatReaction, ReactionTiming.POST);
                    break;
                case StatusBehaviourTimings.OnCombatEnd:
                case StatusBehaviourTimings.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void StartTurnReaction(StartTurnGa startTurnGa)
        {
            if (startTurnGa.starting == CombatLoop.TurnType.Player)
                CheckBehaviour(ComputeTarget());
        }

        private void EndTurnReaction(EndTurnGA endTurnGa)
        {
            if (endTurnGa.ending == CombatLoop.TurnType.Player)
                CheckBehaviour(ComputeTarget());
        }

        private void DealDamageReaction(DealDamageGA dealDamageGa)
        {
            if (dealDamageGa.attacker == cardController && !dealDamageGa.isDamageNegated)
                CheckBehaviour(ComputeTarget(dealDamageGa, true));
        }

        private void ReceiveDamageReaction(DealDamageGA dealDamageGa)
        {
            if (dealDamageGa.target == cardController && !dealDamageGa.isDamageNegated)
                CheckBehaviour(ComputeTarget(dealDamageGa, false));
        }
        
        private void StartCombatReaction(StartTurnGa startTurnGa)
        {
            if (startTurnGa.turnCount == 1)
                CheckBehaviour(ComputeTarget());
        }
        
        private CardController ComputeTarget()
        {
            switch (target)
            {
                case StatusBehaviourTarget.OnMe:
                    return cardController;
                case StatusBehaviourTarget.OnTarget:
                    return cardController;
                case StatusBehaviourTarget.OnRandomAlly:
                    return GetRandomTarget(TargetType.Ally);
                case StatusBehaviourTarget.OnRandomEnemy:
                    return GetRandomTarget(TargetType.Enemy);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private CardController ComputeTarget(DealDamageGA dealDamageGa, bool isAttacker)
        {
            switch (target)
            {
                case StatusBehaviourTarget.OnMe:
                    return cardController;
                case StatusBehaviourTarget.OnTarget:
                    return isAttacker ? dealDamageGa.target : dealDamageGa.attacker;
                case StatusBehaviourTarget.OnRandomAlly:
                    return GetRandomTarget(TargetType.Ally);
                case StatusBehaviourTarget.OnRandomEnemy:
                    return GetRandomTarget(TargetType.Enemy);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private CardController GetRandomTarget(TargetType targetType)
        {
            CardMovement cardTarget = TargetingSystem.instance.RetrieveRandomCard(targetType);

            if (cardTarget == null)
                return null; 
            
            return cardTarget.cardController;
        }

        private void CheckBehaviour(CardController cardTarget)
        {
            switch (behaviour)
            {
                case StatusBehaviour.RemoveOne:
                    RemoveOne(cardTarget);
                    break;
                case StatusBehaviour.RemoveAll:
                    RemoveAll(cardTarget);
                    break;
                case StatusBehaviour.AddOne:
                    AddOne(cardTarget);
                    break;
                case StatusBehaviour.RemoveNone:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void RemoveOne(CardController cardTarget)
        {
            ConsumeStacksGa consume = new ConsumeStacksGa(statusType, 1, cardController, cardTarget);
            ActionSystem.instance.AddReaction(consume);
        }
        
        private void RemoveAll(CardController cardTarget)
        {
            if (!cardController.cardStatus.IsStatusApplied(statusType))
                return;
                
            ConsumeStacksGa consume = new ConsumeStacksGa(statusType, cardController.cardStatus.GetCurrentStackCount(statusType), cardController, cardTarget);
            ActionSystem.instance.AddReaction(consume);
        }
        
        private void AddOne(CardController cardTarget)
        {
            ApplyStatusGa apply = new ApplyStatusGa(statusType, 1, cardController, cardTarget);
            ActionSystem.instance.AddReaction(apply);
        }
    }
}
