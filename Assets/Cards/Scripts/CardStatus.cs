using System;
using System.Collections.Generic;
using System.Linq;
using ActionReaction;
using ActionReaction.Game_Actions;
using BoomLib.Tools;
using Combat.Status;
using Combat.Status.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Combat.CombatLoop;

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
        Poison,
        Dive,
        Corpse,
        Spear,
        Webbed,
        Sonar,
        Moonlight,
        Marker
    }

    public enum StatusBehaviour
    {
        RemoveOne,
        RemoveAll,
        RemoveNone,
        AddOne
    }

    public enum StatusBehaviourTimings
    {
        OnTurnStart,
        OnTurnEnd,
        OnDamageDealt,
        OnDamageReceived,
        OnCombatStart,
        OnCombatEnd,
        None
    }

    public enum StatusBehaviourTarget
    {
        OnMe,
        OnTarget,
        OnRandomAlly,
        OnRandomEnemy
    }
    
    public enum StatusTabModification
    {
        Create,
        Edit,
        Remove
    }

    public class StatusHolder
    {
        public StatusType statusType;
        public int stackCount { get; private set; }
        private StatusController controller;

        public StatusHolder(StatusType statusType, int stackCount, StatusController controller)
        {
            this.statusType = statusType;
            this.stackCount = stackCount;

            if (controller != null)
            {
                this.controller = controller;
                this.controller.AddStack(stackCount);
            }
        }
        
        public void AddStack(int amount)
        {
            stackCount += amount;
            if (controller != null)
                controller.AddStack(amount);
        }
        
        public  void RemoveStack(int amount)
        {
            stackCount -= amount;
            if (controller != null)
                controller.RemoveStack(amount);
        }

        public void AddController(StatusController newController)
        {
            controller = newController;
        }

        public void RemoveController()
        {
            if (controller != null)
                controller.Remove();
        }
    }
    
    public class CardStatus : MonoBehaviour
    {
        [SerializeField] private Image stunEffect;

        [HideInInspector] public UnityEvent<StatusType, StatusTabModification> OnUpdateStatus = new UnityEvent<StatusType, StatusTabModification>();
        
        private CardController cardController;

        private List<StatusHolder> currentStacks = new List<StatusHolder>();
        public List<StatusHolder> CurrentStacks => currentStacks;

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
                CheckForPoisonStacks();
                UpdateStacksAtStartOfTurn();
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
            if (enemyPerformsActionGa.cardController == cardController && IsStatusApplied(StatusType.DoritoCaltrop))
            {
                DealDamageGA doritoCaltrop = new DealDamageGA(1, cardController, cardController);
                ActionSystem.instance.AddReaction(doritoCaltrop);
            }
        }

        private void CheckForPoisonStacks()
        {
            if (IsStatusApplied(StatusType.Poison))
            {
                DealDamageGA poisonDamage = new DealDamageGA(GetCurrentStackCount(StatusType.Poison), null, cardController);
                poisonDamage.bypassArmor = true;
                ActionSystem.instance.AddReaction(poisonDamage);
            }
        }

        private void CheckForCaptureStacksDealDamage()
        {
            if (GetCurrentStackCount(StatusType.Captured) > 0)
            {
                DealDamageGA captureDamage = new DealDamageGA(2, null, cardController);
                ActionSystem.instance.AddReaction(captureDamage);
            }
        }

        private void DealDamageReaction(DealDamageGA dealDamageGa)
        {
            if (dealDamageGa.IsCardTargeted(cardController))
            {
                foreach (StatusHolder statusHolder in currentStacks.ToList())
                {
                    if (!IsStatusApplied(statusHolder.statusType))
                        continue;
                    
                    StatusData statusData = StatusSystem.instance.GetStatusData(statusHolder.statusType);
                
                    if (statusData.behaviourTiming != StatusBehaviourTimings.OnDamageReceived)
                        continue;
                    
                    switch (statusData.behaviour)
                    {
                        case StatusBehaviour.RemoveOne:
                            RemoveOneStack(statusHolder.statusType); 
                            break;
                        case StatusBehaviour.RemoveAll:
                            RemoveAllStacks(statusHolder.statusType);
                            break;
                        case StatusBehaviour.RemoveNone:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            
            if (dealDamageGa.attacker != null && dealDamageGa.attacker == cardController)
            {
                foreach (StatusHolder statusHolder in currentStacks.ToList())
                {
                    if (!IsStatusApplied(statusHolder.statusType))
                        continue;
                
                    StatusData statusData = StatusSystem.instance.GetStatusData(statusHolder.statusType);
                    
                    if (statusData.behaviourTiming != StatusBehaviourTimings.OnDamageDealt)
                        continue;
                    
                    switch (statusData.behaviour)
                    {
                        case StatusBehaviour.RemoveOne:
                            RemoveOneStack(statusHolder.statusType); 
                            break;
                        case StatusBehaviour.RemoveAll:
                            RemoveAllStacks(statusHolder.statusType);
                            break;
                        case StatusBehaviour.RemoveNone:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }
        
        private void UpdateStacksAtStartOfTurn()
        {
            foreach (StatusHolder statusHolder in currentStacks.ToList())
            {
                if (!IsStatusApplied(statusHolder.statusType))
                    continue;
                
                StatusData statusData = StatusSystem.instance.GetStatusData(statusHolder.statusType);
                
                if (statusData.behaviourTiming != StatusBehaviourTimings.OnTurnStart)
                    continue;
                    
                switch (statusData.behaviour)
                {
                    case StatusBehaviour.RemoveOne:
                        RemoveOneStack(statusHolder.statusType); 
                        break;
                    case StatusBehaviour.RemoveAll:
                        RemoveAllStacks(statusHolder.statusType);
                        break;
                    case StatusBehaviour.RemoveNone:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void UpdateStacksAtEndOfTurn()
        {
            foreach (StatusHolder statusHolder in currentStacks.ToList())
            {
                if (!IsStatusApplied(statusHolder.statusType))
                    continue;

                StatusData statusData = StatusSystem.instance.GetStatusData(statusHolder.statusType);
                
                if (statusData.behaviourTiming != StatusBehaviourTimings.OnTurnEnd)
                    continue;
                
                switch (statusData.behaviour)
                {
                    case StatusBehaviour.RemoveOne:
                        RemoveOneStack(statusHolder.statusType); 
                        break;
                    case StatusBehaviour.RemoveAll:
                        RemoveAllStacks(statusHolder.statusType);
                        break;
                    case StatusBehaviour.RemoveNone:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        private void RemoveOneStack(StatusType statusType)
        {
            ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(statusType, 1, cardController, cardController);
            
            if (ActionSystem.instance.IsPerforming)
                ActionSystem.instance.AddReaction(consumeStacksGa);
            else
                ActionSystem.instance.Perform(consumeStacksGa);
        }

        private void RemoveAllStacks(StatusType statusType)
        {
            ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(statusType, GetCurrentStackCount(statusType), cardController, cardController);
            
            if (ActionSystem.instance.IsPerforming)
                ActionSystem.instance.AddReaction(consumeStacksGa);
            else
                ActionSystem.instance.Perform(consumeStacksGa);
        }

        public void ApplyStatusStacks(StatusType statusType, int stacksCount)
        {
            if (stacksCount == 0)
                return;
            
            StatusHolder statusHolder = GetStatusHolder(statusType);
            
            if (statusHolder != null)
            {
                StatusTabModification statusTabModification = statusHolder.stackCount == 0 ? StatusTabModification.Create : StatusTabModification.Edit;
                
                if (statusTabModification == StatusTabModification.Create)
                    statusHolder.AddController(CreateNewController(statusType));
                
                statusHolder.AddStack(stacksCount);
                
                OnUpdateStatus?.Invoke(statusType, statusTabModification);
            }
            else
            {
                StatusHolder newHolder = new StatusHolder(statusType, stacksCount, CreateNewController(statusType));
                currentStacks.Add(newHolder);
                OnUpdateStatus?.Invoke(statusType, StatusTabModification.Create);
            }
        }

        public bool ConsumeStacks(StatusType type, int amount)
        {
            StatusHolder statusHolder = GetStatusHolder(type);
            
            if (statusHolder.stackCount > 0)
            {
                int toBeRemoved = Mathf.Min(statusHolder.stackCount, amount);
                statusHolder.RemoveStack(toBeRemoved);
                
                StatusTabModification statusTabModification = statusHolder.stackCount > 0 ? StatusTabModification.Edit : StatusTabModification.Remove;
                
                if (statusTabModification == StatusTabModification.Remove)
                    statusHolder.RemoveController();
                
                OnUpdateStatus?.Invoke(type, statusTabModification);
                return true;
            }

            return false;
        }
        
        private StatusController CreateNewController(StatusType statusType)
        {
            StatusData data = StatusSystem.instance.GetStatusData(statusType);

            if (data.statusController != null)
            {
                StatusController controller = Instantiate(data.statusController, cardController.statusControllersHolder);
                controller.Setup(cardController, data);
                return controller;
            }

            return null;
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

        public StatusHolder GetStatusHolder(StatusType type)
        {
            return currentStacks.FirstOrDefault(x => x.statusType == type);
        }

        public bool IsStatusApplied(StatusType type)
        {
            StatusHolder statusHolder = GetStatusHolder(type);
            
            return statusHolder != null && statusHolder.stackCount > 0;
        }

        public bool WasStatusEverApplied(StatusType type)
        {
            return GetStatusHolder(type) != null;
        }

        public int GetCurrentStackCount(StatusType type)
        {
            StatusHolder statusHolder = GetStatusHolder(type);
            
            return statusHolder != null ? statusHolder.stackCount : 0;
        }
    }
}
