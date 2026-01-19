using System.Collections;
using System.Collections.Generic;
using Cards.Scripts;
using UnityEngine;

namespace Spells
{
    public interface ISpellController
    {
        public void Setup(CardController controller, SpellData data) {}
        public bool CanCastSpell();
        public void CastSpell(Transform startPosition) {}
        private IEnumerator CastSpellCoroutine(Transform startPosition) { yield break; }
        private IEnumerator SelectTargetAndCast(Transform startPosition) { yield break; }
        private void CancelTargeting() {}
        private IEnumerator CastSpellOnTarget(CardMovement target) { yield break; }
        private IEnumerator CastSpellOnTarget(List<CardMovement> targets) { yield break; }
    }
}
