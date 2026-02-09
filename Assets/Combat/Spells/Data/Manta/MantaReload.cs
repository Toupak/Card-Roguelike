using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using Cards.Scripts;
using Combat.Passives;
using UnityEngine;

namespace Combat.Spells.Data.Manta
{
    public class MantaReload : SpellController
    {
        [SerializeField] private PassiveData mantaPassiveData;
        [SerializeField] private Sprite mantaWithBothTorpedoesSprite;

        private MantaPassive mantaPassive;
        
        public override bool CanCastSpell()
        {
            if (mantaPassive == null)
                ComputeMantaPassive();
            
            return base.CanCastSpell() && mantaPassive != null && mantaPassive.torpedoCount == 0;
        }

        private void ComputeMantaPassive()
        {
            mantaPassive = (MantaPassive)cardController.passiveHolder.GetPassive(mantaPassiveData);
        }

        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            if (mantaPassive == null)
                ComputeMantaPassive();
            
            if (mantaPassive != null)
            {
                mantaPassive.ReloadTorpedoes();
                cardController.SetArtwork(mantaWithBothTorpedoesSprite);
            }
        }
    }
}
