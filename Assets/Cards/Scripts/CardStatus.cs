using System;
using System.Collections.Generic;
using System.Linq;
using ActionReaction;
using ActionReaction.Game_Actions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static CombatLoop.CombatLoop;

namespace Cards.Scripts
{
    public enum StatusType
    {
        None,
        Stun,
        DoritoCaltrop,
        CanisBalisticBullet,
    }
    
    public class CardStatus : MonoBehaviour
    {
        [SerializeField] private Image stunEffect;

        [HideInInspector] public UnityEvent OnUpdateStatus = new UnityEvent();
        
        private CardController cardController;

        public Dictionary<StatusType, int> currentStacks = new Dictionary<StatusType, int>();
        
        public bool IsStun => currentStacks.ContainsKey(StatusType.Stun) && currentStacks[StatusType.Stun] > 0;

        private void Start()
        {
            cardController = GetComponent<CardController>();
            
            stunEffect.gameObject.SetActive(false);       //
            OnUpdateStatus.AddListener(UpdateStunEffect); // Move this to DisplayCardStatusVFX
        }
        
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<EndTurnGA>(EndTurnReaction, ReactionTiming.PRE);
            ActionSystem.SubscribeReaction<EnemyPerformsActionGa>(EnemyPerformsActionReaction, ReactionTiming.POST);
        }
        
        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<EndTurnGA>(EndTurnReaction, ReactionTiming.PRE);
            ActionSystem.UnsubscribeReaction<EnemyPerformsActionGa>(EnemyPerformsActionReaction, ReactionTiming.POST);
        }

        private void EndTurnReaction(EndTurnGA endTurnGa)
        {
            if (IsCorrectTurn(endTurnGa.ending))
                RemoveOneStackOfEachStatus();
        }
        
        private void EnemyPerformsActionReaction(EnemyPerformsActionGa enemyPerformsActionGa)
        {
            if (enemyPerformsActionGa.cardController == cardController && currentStacks.ContainsKey(StatusType.DoritoCaltrop) && currentStacks[StatusType.DoritoCaltrop] > 0)
                cardController.cardHealth.TakeDamage(currentStacks[StatusType.DoritoCaltrop]);
        }

        private void RemoveOneStackOfEachStatus()
        {
            foreach (KeyValuePair<StatusType,int> keyValuePair in currentStacks.ToList())
            {
                if (!IsStatusPersistent(keyValuePair.Key))
                    currentStacks[keyValuePair.Key] = Mathf.Max(keyValuePair.Value - 1, 0);
            }
            
            OnUpdateStatus?.Invoke();
        }

        private bool IsStatusPersistent(StatusType type)
        {
            switch (type)
            {
                case StatusType.Stun:
                    return false;
                case StatusType.DoritoCaltrop:
                    return true;
                case StatusType.CanisBalisticBullet:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public void ApplyStatusStacks(StatusType statusType, int stacksCount)
        {
            if (currentStacks.ContainsKey(statusType))
                currentStacks[statusType] += stacksCount;
            else
                currentStacks.Add(statusType, stacksCount);

            OnUpdateStatus?.Invoke();
        }

        public void ConsumeStacks(StatusType type, int amount)
        {
            if (currentStacks.ContainsKey(type))
                currentStacks[type] = Mathf.Max(currentStacks[type] - amount, 0);
        }
        
        private bool IsCorrectTurn(TurnType startingTurn)
        {
            bool isPlayer = !cardController.cardMovement.IsEnemyCard;

            if (startingTurn == TurnType.Player && isPlayer)
                return true;

            if (startingTurn == TurnType.Enemy && !isPlayer)
                return true;

            return false;
        }
        
        private void UpdateStunEffect()
        {
            stunEffect.gameObject.SetActive(IsStun);
        }
    }
}
