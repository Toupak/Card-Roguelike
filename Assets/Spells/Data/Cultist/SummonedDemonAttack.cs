using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Spells.Data.Cultist
{
    public class SummonedDemonAttack : NecroSpellController
    {
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return ConsumeCorpses(corpseCost);

            foreach (CardMovement target in targets)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                DealDamageGA first = new DealDamageGA(ComputeCurrentDamage(spellData.damage), cardController, target.cardController);
                ActionSystem.instance.Perform(first);
                
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                DealDamageGA second = new DealDamageGA(ComputeCurrentDamage(spellData.damage), cardController, target.cardController);
                ActionSystem.instance.Perform(second);
            }
        }
    }
}
/*
 
 gravedigger_gravedigattack_title
gravedigger_gravedigbuff_title	

necromancer_necromancerspawn_title
necromancer_necromancerspawn	

summoneddemon_summoneddemonattack_title
summoneddemon_summoneddemonattack

*/