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
        BulletAmmo,
        BonusDamage,
        Taunt,
        PermanentBonusDamage,
        HogGroink,
        ReturnDamage,
        GumBoom,
        Weak,
        Obedience,
        GobombCharging,
        Protected,
        Pants,
        Fury,
        Dodge,
        Captured,
        Rage,
        BerserkMode,
        PermanentProtected,
        Armor,
        FreeSpell,
        Vengeance,
        Blood,
        RacoonLastTarget,
        Terror,
        Poison
    }

    public enum StatusEndTurnBehaviour
    {
        RemoveOne,
        RemoveAll,
        RemoveNone,
        RemoveOneAtStartOfTurn,
        RemoveAllAtStartOfTurn
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

        private void Start()
        {
            cardController = GetComponent<CardController>();
            
            stunEffect.gameObject.SetActive(false);       //
            OnUpdateStatus.AddListener(UpdateStunEffect); // Move this to DisplayCardStatusVFX
        }
        
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<EndTurnGA>(EndTurnReaction, ReactionTiming.PRE);
            ActionSystem.SubscribeReaction<StartTurnGa>(StartTurnReaction, ReactionTiming.PRE);
            ActionSystem.SubscribeReaction<EnemyPerformsActionGa>(EnemyPerformsActionReaction, ReactionTiming.POST);
            ActionSystem.SubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.POST);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<EndTurnGA>(EndTurnReaction, ReactionTiming.PRE);
            ActionSystem.UnsubscribeReaction<StartTurnGa>(StartTurnReaction, ReactionTiming.PRE);
            ActionSystem.UnsubscribeReaction<EnemyPerformsActionGa>(EnemyPerformsActionReaction, ReactionTiming.POST);
            ActionSystem.UnsubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.POST);
        }

        private void StartTurnReaction(StartTurnGa startTurnGa)
        {
            if (IsCorrectTurn(startTurnGa.starting))
            {
                UpdateStacksAtStartOfTurn();
                CheckForPoisonStacks();
            }
        }

        private void EndTurnReaction(EndTurnGA endTurnGa)
        {
            if (IsCorrectTurn(endTurnGa.ending))
                UpdateStacksAtEndOfTurn();
            else
                CheckForCaptureStacksDealDamage();
        }

        private void EnemyPerformsActionReaction(EnemyPerformsActionGa enemyPerformsActionGa)
        {
            if (enemyPerformsActionGa.cardController == cardController && currentStacks.ContainsKey(StatusType.DoritoCaltrop) && currentStacks[StatusType.DoritoCaltrop] > 0)
            {
                DealDamageGA doritoCaltrop = new DealDamageGA(1, cardController, cardController);
                ActionSystem.instance.AddReaction(doritoCaltrop);
            }
        }

        private void CheckForPoisonStacks()
        {
            if (IsStatusApplied(StatusType.Poison))
            {
                DealDamageGA poisonDamage = new DealDamageGA(GetCurrentStackCount(StatusType.Poison), cardController, cardController);
                ActionSystem.instance.AddReaction(poisonDamage);
            }
        }

        private void CheckForCaptureStacksDealDamage()
        {
            if (GetCurrentStackCount(StatusType.Captured) > 0)
            {
                DealDamageGA doritoCaltrop = new DealDamageGA(2, cardController, cardController);
                ActionSystem.instance.AddReaction(doritoCaltrop);
            }
        }

        private void DealDamageReaction(DealDamageGA dealDamageGa)
        {
            if (dealDamageGa.target == cardController)
            {
                if (IsStatusApplied(StatusType.Taunt))
                {
                    ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.Taunt, 1, dealDamageGa.attacker, cardController);
                    ActionSystem.instance.AddReaction(consumeStacksGa);
                }

                if (IsStatusApplied(StatusType.Terror))
                {
                    ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.Terror, 1, dealDamageGa.attacker, cardController);
                    ActionSystem.instance.AddReaction(consumeStacksGa);
                }
            }
        }
        
        private void UpdateStacksAtStartOfTurn()
        {
            foreach (KeyValuePair<StatusType,int> keyValuePair in currentStacks.ToList())
            {
                switch (StatusSystem.instance.GetStatusData(keyValuePair.Key).endTurnBehaviour)
                {
                    case StatusEndTurnBehaviour.RemoveOneAtStartOfTurn:
                        RemoveOneStack(keyValuePair); 
                        break;
                    case StatusEndTurnBehaviour.RemoveAllAtStartOfTurn:
                        RemoveAllStacks(keyValuePair);
                        break;
                    case StatusEndTurnBehaviour.RemoveOne:
                    case StatusEndTurnBehaviour.RemoveAll:
                    case StatusEndTurnBehaviour.RemoveNone:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
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
                    case StatusEndTurnBehaviour.RemoveOneAtStartOfTurn:
                    case StatusEndTurnBehaviour.RemoveAllAtStartOfTurn:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        private void RemoveOneStack(KeyValuePair<StatusType,int> keyValuePair)
        {
            ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(keyValuePair.Key, 1, cardController, cardController);
            
            if (ActionSystem.instance.IsPerforming)
                ActionSystem.instance.AddReaction(consumeStacksGa);
            else
                ActionSystem.instance.Perform(consumeStacksGa);
        }

        private void RemoveAllStacks(KeyValuePair<StatusType,int> keyValuePair)
        {
            ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(keyValuePair.Key, currentStacks[keyValuePair.Key], cardController, cardController);
            
            if (ActionSystem.instance.IsPerforming)
                ActionSystem.instance.AddReaction(consumeStacksGa);
            else
                ActionSystem.instance.Perform(consumeStacksGa);
        }

        public void ApplyStatusStacks(StatusType statusType, int stacksCount)
        {
            if (stacksCount == 0)
                return;
            
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

        public bool ConsumeStacks(StatusType type, int amount)
        {
            bool isStackApplied = currentStacks.ContainsKey(type);
            bool wasConsumed = false;

            if (isStackApplied)
            {
                wasConsumed = currentStacks[type] > 0;
                currentStacks[type] = Mathf.Max(currentStacks[type] - amount, 0);
                OnUpdateStatus?.Invoke(type, currentStacks[type] > 0 ? StatusTabModification.Edit : StatusTabModification.Remove);
            }

            return wasConsumed;
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
            stunEffect.gameObject.SetActive(IsStatusApplied(StatusType.Stun));
        }

        public bool IsStatusApplied(StatusType type)
        {
            return currentStacks.ContainsKey(type) && currentStacks[type] > 0;
        }

        public int GetCurrentStackCount(StatusType type)
        {
            if (IsStatusApplied(type))
                return currentStacks[type];

            return 0;
        }
    }
}
