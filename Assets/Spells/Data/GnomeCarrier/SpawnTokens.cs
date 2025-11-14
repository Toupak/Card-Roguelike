using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Spells.Data.GnomeCarrier
{
    public class SpawnTokens : SpellController
    {
        [SerializeField] private CardData tokenData;
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            if (tokenData == null)
                yield break;
            
            for (int i = 0; i < spellData.statusStacksApplied; i++)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                
                SpawnCardGA spawnCardGa = new SpawnCardGA(tokenData, cardController, true);
                ActionSystem.instance.Perform(spawnCardGa);
            }
        }
    }
}
