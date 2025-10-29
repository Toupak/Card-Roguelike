using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Spells.Data;
using UnityEngine;

namespace Spells
{
    public class BasicDamageSpellController : SpellController
    {
        [SerializeField] protected int damage;
        
        protected override IEnumerator CastSpellOnTarget(SpellData spellData, List<CardMovement> targets)
        {
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            Debug.Log($"Cast Basic Damage Spell {spellData.spellName} on targets : ");
            OnCastSpell?.Invoke();
            
            foreach (CardMovement target in targets)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                
                Debug.Log($"Target : {target.cardController.cardData.cardName} / {spellData.targetType}");
                DealDamageGA damageGa = new DealDamageGA(damage, target.cardController);
                ActionSystem.instance.Perform(damageGa);
            }
        }
    }
}
