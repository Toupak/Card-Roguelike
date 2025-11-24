using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Spells.Data.ChildSoldier.Teen
{
    public class TeenSoldierDefense : SpellController
    {
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            ApplyStatusGa applyTaunt = new ApplyStatusGa(StatusType.Taunt, 1, cardController, cardController);
            ActionSystem.instance.Perform(applyTaunt);
            
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            
            ApplyStatusGa applyArmor = new ApplyStatusGa(StatusType.Armor, 1, cardController, cardController);
            ActionSystem.instance.Perform(applyArmor);
        }
    }
}
