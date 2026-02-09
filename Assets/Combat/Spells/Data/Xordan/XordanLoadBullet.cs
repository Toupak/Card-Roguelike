using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.Spells.Data.Xordan
{
    public class XordanLoadBullet : SpellController
    {
        [SerializeField] private SpellData shootSpellData;
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.BulletAmmo, 1, cardController, cardController);
            ActionSystem.instance.Perform(applyStatusGa);
            
            cardController.SetupLeftSpell(shootSpellData);
        }
    }
}
