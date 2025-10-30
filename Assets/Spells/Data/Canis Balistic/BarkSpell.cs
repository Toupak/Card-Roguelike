using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Spells.Data.Canis_Balistic
{
    public class BarkSpell : SpellController
    {
        [SerializeField] protected int damage;

        private int currentBullets = 0;

        public override bool CanCastSpell(SpellData spellData)
        {
            return base.CanCastSpell(spellData) && currentBullets > 0;
        }

        protected override IEnumerator CastSpellOnTarget(SpellData spellData, List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(spellData, targets);
            currentBullets -= 1;

            foreach (CardMovement target in targets)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                
                Debug.Log($"Target : {target.cardController.cardData.cardName} / {spellData.targetType}");
                DealDamageGA damageGa = new DealDamageGA(damage, target.cardController);
                ActionSystem.instance.Perform(damageGa);
            }
        }

        public void LoadBullet()
        {
            currentBullets += 1;
        }
    }
}
