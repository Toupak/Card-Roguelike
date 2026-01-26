using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using Cards.Scripts;
using Combat.Passives;
using UnityEngine;

namespace Combat.Spells.Data.Gardener
{
    public class GardenerWater : SpellController
    {
        private GardenerPotsPassive gardenerPassive = null;
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            if (gardenerPassive == null)
                gardenerPassive = GetPassiveController();

            foreach (CardMovement target in targets)
            {
                yield return gardenerPassive.UseWater(target.cardController.cardData);
            }
        }

        private GardenerPotsPassive GetPassiveController()
        {
            foreach (PassiveController passiveController in cardController.passiveHolder.passives)
            {
                GardenerPotsPassive passive = passiveController.GetComponent<GardenerPotsPassive>();
                if (passive != null)
                    return passive;
            }

            return null;
        }
    }
}
