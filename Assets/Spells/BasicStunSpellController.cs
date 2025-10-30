using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Spells
{
    public class BasicStunSpellController : SpellController
    {
        [SerializeField] protected int stackCount;
        
        protected override IEnumerator CastSpellOnTarget(SpellData spellData, List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(spellData, targets);
            
            foreach (CardMovement target in targets)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                
                Debug.Log($"Target : {target.cardController.cardData.cardName} / {spellData.targetType}");
                StunGa stunGa = new StunGa(stackCount, target.cardController);
                ActionSystem.instance.Perform(stunGa);
            }
        }
    }
}
