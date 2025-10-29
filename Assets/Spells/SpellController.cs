using System;
using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using Cards.Scripts;
using Spells.Data;
using Spells.Targeting;
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
        
        protected Coroutine castSpellRoutine = null;
        public bool IsCasting => castSpellRoutine != null;

        public virtual void Setup(CardController controller)
        {
            cardController = controller;
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
                    yield return CastSpellOnTarget(spellData, cardController.cardMovement);
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

        protected virtual IEnumerator CastSpellOnTarget(SpellData spellData, CardMovement target)
        {
            yield return CastSpellOnTarget(spellData, new List<CardMovement>(){ target });
        }

        protected virtual IEnumerator CastSpellOnTarget(SpellData spellData, List<CardMovement> targets)
        {
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            Debug.Log($"Cast Spell {spellData.spellName} on targets : ");
            OnCastSpell?.Invoke();
            
            foreach (CardMovement target in targets)
            {
                Debug.Log($"Target : {target.cardController.cardData.cardName} / {spellData.targetType}");
            }
        }
    }
}
