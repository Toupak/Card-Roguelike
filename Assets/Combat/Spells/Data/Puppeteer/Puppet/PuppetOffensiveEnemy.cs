using ActionReaction;
using ActionReaction.Game_Actions;
using Combat.Passives;
using UnityEngine;

namespace Combat.Spells.Data.Puppeteer.Puppet
{
    public class PuppetOffensiveEnemy : PassiveController
    {
        [SerializeField] private int damage;
    
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<StartTurnGa>(StartTurnReaction, ReactionTiming.POST);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<StartTurnGa>(StartTurnReaction, ReactionTiming.POST);
        }

        private void StartTurnReaction(StartTurnGa startTurnGa)
        {
            if (startTurnGa.starting == CombatLoop.TurnType.Enemy)
            {
                DealDamageGA puppetAttack = new DealDamageGA(damage, cardController, cardController.tokenParentController);
                ActionSystem.instance.AddReaction(puppetAttack);
            }
        }
    }
}
