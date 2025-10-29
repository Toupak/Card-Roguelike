using System.Collections;
using System.Collections.Generic;
using Cards.Scripts;
using UnityEngine;

namespace Spells
{
    public interface ISpellController
    {
        public void Setup(CardController controller) {}
        public bool CanCastSpell();
        public void CastSpell(Transform startPosition, SpellData spellData) {}
        private IEnumerator CastSpellCoroutine(Transform startPosition, SpellData spellData) { yield break; }
        private IEnumerator SelectTargetAndCast(Transform startPosition, SpellData spellData) { yield break; }
        private void CancelTargeting() {}
        private IEnumerator CastSpellOnTarget(SpellData spellData, CardMovement target) { yield break; }
        private IEnumerator CastSpellOnTarget(SpellData spellData, List<CardMovement> targets) { yield break; }
    }
}
