using System;
using System.Collections.Generic;
using System.Linq;
using ActionReaction;
using ActionReaction.Game_Actions;
using Status;
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
        BonusDamage,
    }

    public enum StatusEndTurnBehaviour
    {
        RemoveOne,
        RemoveAll,
        RemoveNone
    }
    
    public enum StatusTabModification
    {
        Create,
        Edit,
        Remove
    }
    
    public class CardStatus : MonoBehaviour
    {
        [SerializeField] private Image stunEffect;

        [HideInInspector] public UnityEvent<StatusType, StatusTabModification> OnUpdateStatus = new UnityEvent<StatusType, StatusTabModification>();
        
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
                UpdateStacksAtEndOfTurn();
        }
        
        private void EnemyPerformsActionReaction(EnemyPerformsActionGa enemyPerformsActionGa)
        {
            if (enemyPerformsActionGa.cardController == cardController && currentStacks.ContainsKey(StatusType.DoritoCaltrop) && currentStacks[StatusType.DoritoCaltrop] > 0)
                cardController.cardHealth.TakeDamage(currentStacks[StatusType.DoritoCaltrop]);
        }

        private void UpdateStacksAtEndOfTurn()
        {
            foreach (KeyValuePair<StatusType,int> keyValuePair in currentStacks.ToList())
            {
                switch (StatusSystem.instance.GetStatusData(keyValuePair.Key).endTurnBehaviour)
                {
                    case StatusEndTurnBehaviour.RemoveOne:
                        RemoveOneStack(keyValuePair); 
                        break;
                    case StatusEndTurnBehaviour.RemoveAll:
                        RemoveAllStacks(keyValuePair);
                        break;
                    case StatusEndTurnBehaviour.RemoveNone:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        private void RemoveOneStack(KeyValuePair<StatusType,int> keyValuePair)
        {
            currentStacks[keyValuePair.Key] = Mathf.Max(keyValuePair.Value - 1, 0);
            OnUpdateStatus?.Invoke(keyValuePair.Key, currentStacks[keyValuePair.Key] > 0 ? StatusTabModification.Edit : StatusTabModification.Remove);
        }

        private void RemoveAllStacks(KeyValuePair<StatusType,int> keyValuePair)
        {
            currentStacks[keyValuePair.Key] = 0;
            OnUpdateStatus?.Invoke(keyValuePair.Key, StatusTabModification.Remove);
        }

        public void ApplyStatusStacks(StatusType statusType, int stacksCount)
        {
            if (currentStacks.ContainsKey(statusType))
            {
                currentStacks[statusType] += stacksCount;
                OnUpdateStatus?.Invoke(statusType, currentStacks[statusType] == stacksCount ? StatusTabModification.Create : StatusTabModification.Edit);
            }
            else
            {
                currentStacks.Add(statusType, stacksCount);
                OnUpdateStatus?.Invoke(statusType, StatusTabModification.Create);
            }
        }

        public void ConsumeStacks(StatusType type, int amount)
        {
            if (currentStacks.ContainsKey(type))
                currentStacks[type] = Mathf.Max(currentStacks[type] - amount, 0);
            
            OnUpdateStatus?.Invoke(type, currentStacks[type] > 0 ? StatusTabModification.Edit : StatusTabModification.Remove);
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
        
        private void UpdateStunEffect(StatusType type, StatusTabModification statusTabModification) // Move this to DisplayCardStatusVFX
        {
            stunEffect.gameObject.SetActive(IsStun);
        }
    }
}
