using System;
using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Spells.Targeting;
using Status;
using UnityEngine;
using UnityEngine.Events;

namespace Spells
{
    public class SpellController : MonoBehaviour, ISpellController
    {
        public static UnityEvent OnStartCastingSpell = new UnityEvent();
        public static UnityEvent OnCastSpell = new UnityEvent();
        public static UnityEvent OnCancelSpell = new UnityEvent();
        
        protected CardController cardController;
        protected SpellButton otherSpellButton;
        
        protected Coroutine castSpellRoutine = null;
        public bool IsCasting => castSpellRoutine != null;
        
        public bool HasCastedThisTurn { get; protected set; }

        public virtual void Setup(CardController controller, SpellData spellData, SpellButton otherSpell)
        {
            cardController = controller;
            otherSpellButton = otherSpell;
        }

        public virtual bool CanCastSpell(SpellData spellData)
        {
            return !HasCastedThisTurn && EnergyController.instance.CheckForEnergy(spellData.energyCost) && !cardController.cardStatus.IsStun;
        }

        public virtual void CastSpell(Transform startPosition, SpellData spellData)
        {
            if (castSpellRoutine != null)
                CancelTargeting();
            else
                castSpellRoutine = StartCoroutine(CastSpellCoroutine(startPosition, spellData));
            OnStartCastingSpell?.Invoke();
        }

        protected virtual IEnumerator CastSpellCoroutine(Transform startPosition, SpellData spellData)
        {
            Debug.Log("Start Casting Spell");
            
            switch (spellData.targetType)
            {
                case TargetType.None:
                case TargetType.Ally:
                case TargetType.Enemy:
                    yield return SelectTargetAndCast(startPosition, spellData);
                    break;
                case TargetType.Self:
                    yield return CastSpellOnSelf(spellData, cardController.cardMovement);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            castSpellRoutine = null;
        }

        protected virtual IEnumerator SelectTargetAndCast(Transform startPosition, SpellData spellData)
        {
            yield return TargetingSystem.instance.SelectTargets(cardController.cardMovement, startPosition, spellData.targetType, spellData.targetingMode, spellData.targetCount);
            if (TargetingSystem.instance.IsCanceled)
                CancelTargeting();
            else
                yield return CastSpellOnTarget(spellData, TargetingSystem.instance.Targets); 
        }

        protected virtual void CancelTargeting()
        {
            StopAllCoroutines();
            castSpellRoutine = null;
            OnCancelSpell?.Invoke();
            Debug.Log("Cancel Targeting");
        }

        protected virtual IEnumerator CastSpellOnSelf(SpellData spellData, CardMovement target)
        {
            yield return CastSpellOnTarget(spellData, new List<CardMovement>(){ target });
        }

        protected virtual IEnumerator CastSpellOnTarget(SpellData spellData, List<CardMovement> targets)
        {
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            HasCastedThisTurn = true;
            Debug.Log($"Cast Spell {spellData.spellName} on targets : ");
            OnCastSpell?.Invoke();
            EnergyController.instance.RemoveEnergy(spellData.energyCost);
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
        }
        
        protected virtual void UnsubscribeReactions()
        {
            ActionSystem.UnsubscribeReaction<StartTurnGa>(EndTurnRefreshCooldownReaction, ReactionTiming.PRE);
        }

        private void EndTurnRefreshCooldownReaction(StartTurnGa startTurnGa)
        {
            if (startTurnGa.starting == CombatLoop.CombatLoop.TurnType.Player)
                HasCastedThisTurn = false;
        }

        protected virtual int ComputeCurrentDamage(int spellDamage)
        {
            int bonus = 0;

            if (StatusSystem.instance.IsCardAfflictedByStatus(cardController, StatusType.BonusDamage))
                bonus += cardController.cardStatus.currentStacks[StatusType.BonusDamage];

            return spellDamage + bonus;
        }
    }
}
