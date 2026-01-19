using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.Spells.Data.GnomeCarrier
{
    public class SpawnTokens : SpellController
    {
        [SerializeField] private CardData tokenData;
        
        public override bool CanCastSpell()
        {
            return base.CanCastSpell() && ComputeCurrentDamage(spellData.statusStacksApplied) > 0;
        }
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            if (tokenData == null)
                yield break;

            int tokenSpawnCount = Mathf.Min(5, ComputeCurrentDamage(spellData.statusStacksApplied));
            
            for (int i = 0; i < tokenSpawnCount; i++)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                
                SpawnCardGA spawnCardGa = new SpawnCardGA(tokenData, cardController, true);
                ActionSystem.instance.Perform(spawnCardGa);
            }
        }
    }
}
