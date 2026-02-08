using System;
using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.EnergyBar;
using Combat.Spells.Targeting;
using Localization;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Combat.Spells
{
    public class SpellController : MonoBehaviour, ISpellController
    {
        [HideInInspector] public UnityEvent OnStartCastingSpell = new UnityEvent();
        [HideInInspector] public UnityEvent OnCastSpell = new UnityEvent();
        public static UnityEvent OnCancelSpell = new UnityEvent();
        
        public CardController cardController { get; private set; }
        public SpellData spellData { get; protected set; }
        protected SpellButton thisSpellButton;
        protected SpellButton otherSpellButton;
        
        protected Coroutine castSpellRoutine = null;
        public bool IsCasting => castSpellRoutine != null;
        
        public bool IsShiny { get; protected set; }
        public bool HasCastedThisTurn { get; protected set; }

        protected bool isLocking;
        public int spellIndex => thisSpellButton.spellIndex;

        public virtual void Setup(CardController controller, SpellData data, SpellButton attacheSpellButton, SpellButton otherSpell)
        {
            cardController = controller;
            spellData = data;
            thisSpellButton = attacheSpellButton;
            otherSpellButton = otherSpell;
            
            cardController.OnKillCard?.AddListener(() =>
            {
                if (isLocking)
                    ActionSystem.instance.SetLock(false);
            });
        }

        public virtual bool CanCastSpell()
        {
            if (CombatLoop.instance == null || CombatLoop.instance.currentTurn != CombatLoop.TurnType.Player)
                return false;
            
            if (CombatLoop.instance == null || CombatLoop.instance.currentTurn == CombatLoop.TurnType.Preparation)
                return false;
            
            if (cardController.cardStatus.IsStatusApplied(StatusType.Stun))
                return false;

            if (cardController.cardStatus.IsStatusApplied(StatusType.Captured) || cardController.cardStatus.IsStatusApplied(StatusType.Dive))
                return false;
            
            if (IsShiny)
                return true;
            
            if (HasCastedThisTurn)
                return false;

            if (!EnergyController.instance.CheckForEnergy(spellData.energyCost) && !cardController.cardStatus.IsStatusApplied(StatusType.FreeSpell))
                return false;

            return true;
        }

        public virtual void CastSpell(Transform startPosition)
        {
            if (castSpellRoutine != null)
                CancelTargeting();
            else
                castSpellRoutine = StartCoroutine(CastSpellCoroutine(startPosition));
            OnStartCastingSpell?.Invoke();
        }

        protected virtual IEnumerator CastSpellCoroutine(Transform startPosition)
        {
            Debug.Log("Start Casting Spell");
            
            switch (spellData.targetType)
            {
                case TargetType.None:
                case TargetType.Ally:
                case TargetType.Enemy:
                    yield return SelectTargetAndCast(startPosition);
                    break;
                case TargetType.Self:
                    yield return CastSpellOnSelf(cardController.cardMovement);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            castSpellRoutine = null;//BEWARE ! do not forget to reset the coroutine when overriding this method
        }

        protected virtual IEnumerator SelectTargetAndCast(Transform startPosition)
        {
            yield return TargetingSystem.instance.SelectTargets(cardController.cardMovement, startPosition, spellData.targetType, spellData.targetingMode, ComputeCurrentTargetCount(spellData.targetCount), spellData.targetTokens);
            if (TargetingSystem.instance.IsCanceled)
                CancelTargeting();
            else
                yield return LockActionAndCast(TargetingSystem.instance.Targets);
        }

        protected virtual void CancelTargeting()
        {
            StopAllCoroutines();
            castSpellRoutine = null;
            OnCancelSpell?.Invoke();
            Debug.Log("Cancel Targeting");
        }

        protected virtual IEnumerator CastSpellOnSelf(CardMovement target)
        {
            yield return LockActionAndCast(new List<CardMovement>(){ target });
        }

        protected virtual IEnumerator LockActionAndCast(List<CardMovement> targets)
        {
            yield return new WaitWhile(() => ActionSystem.instance.isLocked);
            isLocking = true;
            ActionSystem.instance.SetLock(true);
            yield return CastSpellOnTarget(targets);
            ActionSystem.instance.SetLock(false);
            isLocking = false;
        }

        protected virtual IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            //yield return base.CastSpellOnTarget(targets);
            //yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            yield return ConsumeEnergy(spellData.energyCost);
            SetShinyState(false);
            HasCastedThisTurn = !spellData.hasNoCooldown;
            OnCastSpell?.Invoke();
            Debug.Log($"Cast Spell {spellData.spellName} on targets : ");
        }

        protected virtual IEnumerator ConsumeEnergy(int cost)
        {
            if (cardController.cardStatus.IsStatusApplied(StatusType.FreeSpell))
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.FreeSpell, 1, cardController, cardController);
                ActionSystem.instance.Perform(consumeStacksGa);
            }
            else if (!IsShiny && cost > 0)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                ConsumeEnergyGa consumeEnergyGa = new ConsumeEnergyGa(cost, this);
                ActionSystem.instance.Perform(consumeEnergyGa);
            }
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
        }

        private void OnEnable()
        {
            SubscribeReactions();
        }

        private void OnDisable()
        {
            UnsubscribeReactions();
        }

        protected virtual void SubscribeReactions()
        {
            ActionSystem.SubscribeReaction<StartTurnGa>(EndTurnRefreshCooldownReaction, ReactionTiming.PRE);
            ActionSystem.SubscribeReaction<RefreshCooldownGA>(RefreshCooldownReaction, ReactionTiming.POST);
        }

        protected virtual void UnsubscribeReactions()
        {
            ActionSystem.UnsubscribeReaction<StartTurnGa>(EndTurnRefreshCooldownReaction, ReactionTiming.PRE);
            ActionSystem.UnsubscribeReaction<RefreshCooldownGA>(RefreshCooldownReaction, ReactionTiming.POST);
        }

        protected virtual void EndTurnRefreshCooldownReaction(StartTurnGa startTurnGa)
        {
            if (startTurnGa.starting == CombatLoop.TurnType.Player)
                RefreshCooldown();
            SetShinyState(false);
        }
        
        protected virtual void RefreshCooldownReaction(RefreshCooldownGA refreshCooldownGa)
        {
            if (refreshCooldownGa.target == cardController)
                RefreshCooldown();
        }

        public virtual void RefreshCooldown()
        {
            HasCastedThisTurn = false;
        }

        public virtual int ComputeCurrentDamage(int spellDamage)
        {
            return cardController.ComputeCurrentDamage(spellDamage);
        }

        public virtual int ComputeCurrentTargetCount(int count)
        {
            return cardController.ComputeCurrentTargetCount(count);
        }
        
        public virtual int ComputeEnergyCost()
        {
            if (IsShiny || cardController.cardStatus.IsStatusApplied(StatusType.FreeSpell))
                return 0;

            return spellData.energyCost;
        }

        public virtual string ComputeTooltipTitle()
        {
            return LocalizationSystem.instance.GetSpellTitle(cardController.cardData.localizationKey, spellData.localizationKey);
        }
        
        public virtual string ComputeTooltipDescription()
        {
            string description = LocalizationSystem.instance.GetSpellDescription(cardController.cardData.localizationKey, spellData.localizationKey);

            int damage = ComputeCurrentDamage(spellData.damage);
            LocalizationSystem.TextDisplayStyle style = LocalizationSystem.instance.ComputeTextDisplayStyle(spellData.damage, damage);

            description = LocalizationSystem.instance.CheckForDamageInText(description, damage.ToString(), style);

            return description;
        }

        public void SetShinyState(bool newState)
        {
            if (IsShiny == newState)
                return;

            IsShiny = newState;
        }

        protected CardController PickRandomTarget(List<CardMovement> targets)
        {
            return targets[Random.Range(0, targets.Count)].cardController;
        }
    }
}
