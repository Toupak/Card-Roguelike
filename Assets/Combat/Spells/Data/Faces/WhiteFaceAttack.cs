using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.Spells.Data.Faces
{
    public class WhiteFaceAttack : SpellController
    {
        [SerializeField] private List<CardData> tokenData;
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            int index = Random.Range(0, tokenData.Count);
            SpawnCardGA spawnCardGa = new SpawnCardGA(tokenData[index], cardController, true);
            ActionSystem.instance.Perform(spawnCardGa);
        }
    }
}
