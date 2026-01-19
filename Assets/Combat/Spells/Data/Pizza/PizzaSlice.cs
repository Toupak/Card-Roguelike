using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.Spells.Data.Pizza
{
    public class PizzaSlice : SpellController
    {
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            foreach (CardMovement target in targets)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                RefreshCooldownGA refreshCooldownGa = new RefreshCooldownGA(cardController, target.cardController);
                ActionSystem.instance.Perform(refreshCooldownGa);
            }
        }
    }
}
