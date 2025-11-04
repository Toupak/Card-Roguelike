using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Spells.Data.Gumball
{
    public class GumballTaunt : SpellController
    {
        protected override IEnumerator CastSpellOnTarget(SpellData spellData, List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(spellData, targets);
            
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            ApplyStatusGa taunt = new ApplyStatusGa(StatusType.Taunt, 1, cardController, cardController);
            ActionSystem.instance.Perform(taunt);
            
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            ApplyStatusGa selfStun = new ApplyStatusGa(StatusType.Stun, 2, cardController, cardController);
            ActionSystem.instance.Perform(selfStun);
        }
    }
}
