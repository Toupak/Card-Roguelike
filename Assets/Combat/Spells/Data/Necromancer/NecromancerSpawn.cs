using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.EnergyBar;
using UnityEngine;

namespace Combat.Spells.Data.Necromancer
{
    public class NecromancerSpawn : NecroSpellController
    {
        [SerializeField] private List<CardData> skeletonsData;

        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            yield return ConsumeCorpses(corpseCost);

            int currentEnergy = Mathf.Min(EnergyController.instance.currentEnergy, skeletonsData.Count - 1);

            SpawnCardGA spawnCardGa = new SpawnCardGA(skeletonsData[currentEnergy], cardController);
            ActionSystem.instance.Perform(spawnCardGa);
        }
    }
}
