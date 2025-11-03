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
        public override void Setup(CardController controller, SpellData spellData, SpellButton otherSpell)
        {
            base.Setup(controller, spellData, otherSpell);
            otherSpell.OnCastSpell.AddListener(() => HasCastedThisTurn = true);
        }
        
        public override bool CanCastSpell(SpellData spellData)
        {
            return base.CanCastSpell(spellData) && StatusSystem.instance.IsCardAfflictedByStatus(cardController, StatusType.CanisBalisticBullet);
        }

        protected override IEnumerator CastSpellOnTarget(SpellData spellData, List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(spellData, targets);
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
