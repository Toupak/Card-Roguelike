using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Spells.Data.Nachito
{
    public class SourCreamSpell : SpellController
    {
        [SerializeField] private int damage;
        
        private DoritoCaltrops doritoSpell = null;
        
        public override bool CanCastSpell(SpellData spellData)
        {
            if (doritoSpell == null)
                doritoSpell = otherSpellButton.GetComponentInChildren<DoritoCaltrops>();
            
            return base.CanCastSpell(spellData) && doritoSpell != null && doritoSpell.HasTargets;
        }
        
        protected override IEnumerator CastSpellOnTarget(SpellData spellData, List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(spellData, targets);

            if (doritoSpell == null)
                doritoSpell = otherSpellButton.GetComponentInChildren<DoritoCaltrops>();
            
            if (doritoSpell == null || !doritoSpell.HasTargets)
                yield break;

            foreach (KeyValuePair<CardController,int> keyValuePair in doritoSpell.stacksDictionary)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                
                DealDamageGA damageGa = new DealDamageGA(damage, keyValuePair.Key);
                ActionSystem.instance.Perform(damageGa);
            }
            
            doritoSpell.ClearAllStacks();
        }
    }
}
