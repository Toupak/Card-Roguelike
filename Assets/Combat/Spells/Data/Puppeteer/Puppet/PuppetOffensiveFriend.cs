using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;
using UnityEngine;

namespace Combat.Spells.Data.Puppeteer.Puppet
{
    public class PuppetOffensiveFriend : PassiveController
    {
        [SerializeField] private int damage;
    
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.POST);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.POST);
        }

        private void DealDamageReaction(DealDamageGA dealDamageGa)
        {
            if (dealDamageGa.attacker != null && dealDamageGa.attacker == cardController.tokenParentController)
            {
                foreach (DealDamageGA.DamagePackage package in dealDamageGa.packages)
                {
                    if (!package.isDamageNegated)
                    {
                        DealDamageGA puppetAttack = new DealDamageGA(damage, cardController, package.target);
                        ActionSystem.instance.AddReaction(puppetAttack);
                    }
                }
            }
        }
    }
}
