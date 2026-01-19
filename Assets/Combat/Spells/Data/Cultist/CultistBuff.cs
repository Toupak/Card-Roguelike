using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Spells.Targeting;
using UnityEngine;

namespace Combat.Spells.Data.Cultist
{
    public class CultistBuff : NecroSpellController
    {
        [SerializeField] private CardData demonData;

        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            CardController demon = TargetingSystem.instance.RetrieveCard(demonData).cardController;
            
            if (demon != null)
            {
                yield return base.CastSpellOnTarget(targets);
                yield return ConsumeCorpses(corpseCost);

                ApplyStatusGa applyStatusGa = new ApplyStatusGa(spellData.inflictStatus, spellData.statusStacksApplied, cardController, demon);
                ActionSystem.instance.Perform(applyStatusGa);
            }
        }
    }
}
