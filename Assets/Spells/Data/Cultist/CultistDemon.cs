using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Spells.Data.Cultist
{
    public class CultistDemon : NecroSpellController
    {
        [SerializeField] private CardData demonData;

        [Space]
        [SerializeField] private SpellData nextSpellData;
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            yield return ConsumeCorpses(corpseCost);

            SpawnCardGA spawnCardGa = new SpawnCardGA(demonData, cardController);
            ActionSystem.instance.Perform(spawnCardGa);
            
            cardController.SetupRightSpell(nextSpellData);
        }
    }
}
