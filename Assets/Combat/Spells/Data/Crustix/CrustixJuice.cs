using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Spells.Data.Crustix
{
    public class CrustixJuice : SpellController
    {
        protected override IEnumerator CastSpellCoroutine(Transform startPosition)
        {
            bool isHiddenInShell = cardController.cardStatus.IsStatusApplied(StatusType.ReturnDamage);
            
            if (isHiddenInShell)
                yield return CastSpellOnSelf(cardController.cardMovement);
            else
                yield return SelectTargetAndCast(startPosition);
            
            castSpellRoutine = null;
        }

        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            bool isHiddenInShell = cardController.cardStatus.IsStatusApplied(StatusType.ReturnDamage);

            if (isHiddenInShell)
                yield return TauntEnemies();
            else
                yield return AttackEnemies(targets);
        }
        
        private IEnumerator TauntEnemies()
        {
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            ApplyStatusGa applyStatusGa = new ApplyStatusGa(spellData.inflictStatus, spellData.statusStacksApplied, cardController, cardController);
            ActionSystem.instance.Perform(applyStatusGa);
        }

        private IEnumerator AttackEnemies(List<CardMovement> targets)
        {
            foreach (CardMovement target in targets)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                DealDamageGA damageGa = new DealDamageGA(ComputeCurrentDamage(spellData.damage), cardController, target.cardController);
                ActionSystem.instance.Perform(damageGa);
            }
        }
    }
}
