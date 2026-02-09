using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

namespace Combat.Spells.Data.MachineGun
{
    public class MGShoot : SpellController
    {
        public override bool CanCastSpell()
        {
            return base.CanCastSpell() && cardController.cardStatus.IsStatusApplied(StatusType.BulletAmmo);
        }
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            int damage = spellData.damage;
            int bullets = cardController.cardStatus.currentStacks[StatusType.BulletAmmo];
            
            List<CardMovement> markedTargets = targets.Where((c) => c.cardController.cardStatus.IsStatusApplied(StatusType.Marker)).ToList();
            
            for (int i = 0; i < bullets; i++)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                CardController target;
                if (markedTargets.Count > 0 && markedTargets.Where((c) => c != null && !c.cardController.cardHealth.IsDead).ToList().Count > 0)
                    target = PickRandomTarget(markedTargets);
                else
                    target = PickRandomTarget(targets);
                    
                DealDamageGA dealDamageGa = new DealDamageGA(ComputeCurrentDamage(damage), cardController, target);
                ActionSystem.instance.Perform(dealDamageGa);
                
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.BulletAmmo, 1, cardController, cardController);
                ActionSystem.instance.Perform(consumeStacksGa);
                
                damage = Mathf.Min(3, damage + 1);
            }
        }
    }
}
