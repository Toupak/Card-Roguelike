using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.Spells.Data.Tinkerer
{
    public class SpawnTrinket : SpellController
    {
        [SerializeField] private CardData trinketData;
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            SpawnCardGA spawnCardGa = new SpawnCardGA(trinketData, cardController, true);
            ActionSystem.instance.Perform(spawnCardGa);
        }
    }
}
