using System;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Items.Frames.data
{
    public class BasicStatusFrameController : FrameController
    {
        [SerializeField] private StatusType statusType;
        [SerializeField] private StatusBehaviour behaviour;
        [SerializeField] private StatusBehaviourTimings timing;
        
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
            if (startTurnGa.starting == CombatLoop.CombatLoop.TurnType.Player)
                CheckBehaviour();
        }
        
        private void EndTurnReaction(EndTurnGA endTurnGa)
        {
            if (endTurnGa.ending == CombatLoop.CombatLoop.TurnType.Player)
                CheckBehaviour();
        }
        
        private void DealDamageReaction(DealDamageGA dealDamageGa)
        {
            if (dealDamageGa.attacker == cardController && !dealDamageGa.isDamageNegated)
                CheckBehaviour();
        }
        
        private void ReceiveDamageReaction(DealDamageGA dealDamageGa)
        {
            if (dealDamageGa.target == cardController && !dealDamageGa.isDamageNegated)
                CheckBehaviour();
        }
        
        private void StartCombatReaction(StartTurnGa startTurnGa)
        {
            if (startTurnGa.turnCount == 1)
                CheckBehaviour();
        }

        private void CheckBehaviour()
        {
            switch (behaviour)
            {
                case StatusBehaviour.RemoveOne:
                    RemoveOne();
                    break;
                case StatusBehaviour.RemoveAll:
                    RemoveAll();
                    break;
                case StatusBehaviour.AddOne:
                    AddOne();
                    break;
                case StatusBehaviour.RemoveNone:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void RemoveOne()
        {
            ConsumeStacksGa consume = new ConsumeStacksGa(statusType, 1, cardController, cardController);
            ActionSystem.instance.AddReaction(consume);
        }
        
        private void RemoveAll()
        {
            if (!cardController.cardStatus.IsStatusApplied(statusType))
                return;
                
            ConsumeStacksGa consume = new ConsumeStacksGa(statusType, cardController.cardStatus.GetCurrentStackCount(statusType), cardController, cardController);
            ActionSystem.instance.AddReaction(consume);
        }
        
        private void AddOne()
        {
            ApplyStatusGa apply = new ApplyStatusGa(statusType, 1, cardController, cardController);
            ActionSystem.instance.AddReaction(apply);
        }
    }
}
