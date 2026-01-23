using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using BoomLib.Tools;
using Cards.Scripts;
using Localization;
using UnityEngine;

namespace Combat.Spells.Data.BatMerchant
{
    public class BatMerchantDamage : SpellController
    { 
        protected override void SubscribeReactions()
        {
            base.SubscribeReactions();
            ActionSystem.SubscribeReaction<DeathGA>(CheckForKillRefresh, ReactionTiming.POST);
        }

        protected override void UnsubscribeReactions()
        {
            base.UnsubscribeReactions();
            ActionSystem.UnsubscribeReaction<DeathGA>(CheckForKillRefresh, ReactionTiming.POST);
        }
        
        private void CheckForKillRefresh(DeathGA deathGa)
        {
            if (deathGa.killer == cardController)
                SetShinyState(true);
        }

        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            SetShinyState(false);
            
            bool isSplitAttack = targets.Count > 1 && Tools.RandomBool();
            int targetCount = Mathf.Min(targets.Count, ComputeCurrentTargetCount(isSplitAttack ? 2 : 1));
            int damage = isSplitAttack ? spellData.damage : spellData.damage * 2;

            for (int i = 0; i < targetCount; i++)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                int RandomEnemy = Random.Range(0, targets.Count);

                DealDamageGA dealDamageGa = new DealDamageGA(ComputeCurrentDamage(damage), cardController, targets[RandomEnemy].cardController);
                ActionSystem.instance.Perform(dealDamageGa);
                
                targets.RemoveAt(RandomEnemy);
            }
        }
        
        public override string ComputeTooltipDescription()
        {
            string description = base.ComputeTooltipDescription();

            int damage_1 = ComputeCurrentDamage(spellData.damage * 2);
            int damage_2 = ComputeCurrentDamage(spellData.damage);
            LocalizationSystem.TextDisplayStyle style = LocalizationSystem.instance.ComputeTextDisplayStyle(spellData.damage, damage_2);

            description = LocalizationSystem.instance.CheckForDamageInText(description, damage_1.ToString(), style, "$d1$");
            description = LocalizationSystem.instance.CheckForDamageInText(description, damage_2.ToString(), style, "$d2$");

            return description;
        }
    }
}
