using ActionReaction;
using ActionReaction.Game_Actions;
using Combat.Passives;
using UnityEngine;

namespace Combat.Spells.Data.Puppeteer.Puppet
{
    public class PuppetDefensiveFriend : PassiveController
    {
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.PRE);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.PRE);
        }

        private void DealDamageReaction(DealDamageGA dealDamageGa)
        {
            Debug.Log("Protection check");
            if (dealDamageGa.target != null && dealDamageGa.target == cardController.tokenParentController)
            {
                Debug.Log("Switch Target");
                dealDamageGa.SwitchTarget(cardController);
            }
        }
    }
}
