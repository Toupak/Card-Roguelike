using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using CombatLoop;
using Status;
using UnityEngine;

namespace Spells.Data.Canis_Balistic
{
    public class BarkSpell : SpellController
    {
        public override void Setup(CardController controller, SpellData data, SpellButton attacheSpellButton, SpellButton otherSpell)
        {
            base.Setup(controller, data, attacheSpellButton, otherSpell);
            otherSpell.OnCastSpell.AddListener(() => HasCastedThisTurn = true);
        }
        
        public override bool CanCastSpell()
        {
            if (!base.CanCastSpell())
                return false;
            
            return StatusSystem.instance.IsCardAfflictedByStatus(cardController, StatusType.CanisBalisticBullet);
        }

        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            HasCastedThisTurn = false;
            
            ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.CanisBalisticBullet, 1, cardController, cardController);
            ActionSystem.instance.Perform(consumeStacksGa);
            
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            foreach (CardMovement target in targets)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                
                Debug.Log($"Target : {target.cardController.cardData.cardName} / {spellData.targetType}");
                DealDamageGA damageGa = new DealDamageGA(ComputeCurrentDamage(spellData.damage), cardController, target.cardController);
                ActionSystem.instance.Perform(damageGa);
            }
        }
    }
}
