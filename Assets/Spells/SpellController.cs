using System;
using System.Collections;
using System.Collections.Generic;
using Cards.Scripts;
using Spells.Data;
using Spells.Targeting;
using UnityEngine;
using UnityEngine.Events;

namespace Spells
{
    public class SpellController : MonoBehaviour
    {
        [SerializeField] private Transform thisCard;

        public static UnityEvent OnStartCastingSpell = new UnityEvent();
        public static UnityEvent OnCastSpell = new UnityEvent();
        public static UnityEvent OnCancelSpell = new UnityEvent();
        
        private Coroutine castSpellRoutine = null;
        public bool IsCasting => castSpellRoutine != null;
        
        public void CastSpell(Transform startPosition, SpellData spellData)
        {
            if (castSpellRoutine != null)
                CancelTargeting();
            else
                castSpellRoutine = StartCoroutine(CastSpellCoroutine(startPosition, spellData));
            OnStartCastingSpell?.Invoke();
        }

        private IEnumerator CastSpellCoroutine(Transform startPosition, SpellData spellData)
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
                    CastSpellOnTarget(spellData, thisCard);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            castSpellRoutine = null;
        }

        private IEnumerator SelectTargetAndCast(Transform startPosition, SpellData spellData)
        {
            yield return TargetingSystem.instance.SelectTargets(startPosition, spellData.targetType, spellData.targetingMode, spellData.targetCount);
            if (TargetingSystem.instance.IsCanceled)
                CancelTargeting();
            else
                CastSpellOnTarget(spellData, TargetingSystem.instance.Targets); 
        }

        private void CancelTargeting()
        {
            StopAllCoroutines();
            castSpellRoutine = null;
            OnCancelSpell?.Invoke();
            Debug.Log("Cancel Targeting");
        }

        private void CastSpellOnTarget(SpellData spellData, Transform target)
        {
            CastSpellOnTarget(spellData, new List<Transform>(){ target });
        }

        private void CastSpellOnTarget(SpellData spellData, List<Transform> targets)
        {
            Debug.Log($"Cast Spell {spellData.spellName} on targets : ");
            foreach (Transform target in targets)
            {
                Debug.Log($"Target : {target.gameObject.name}");
                if (spellData.targetType == TargetType.Ally)
                    DebugHealDamage(target);
                else
                    DebugDealDamage(target);
            }
            OnCastSpell?.Invoke();
        }

        private void DebugDealDamage(Transform target)
        {
            //target.GetComponent<CardMovement>().cardController.cardHealth.TakeDamage(5);
        }
        
        private void DebugHealDamage(Transform target)
        {
            //target.GetComponent<CardMovement>().cardController.cardHealth.HealDamage(5);
        }
    }
}
