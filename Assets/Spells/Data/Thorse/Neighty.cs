using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Passives;
using Spells.Targeting;
using UnityEngine;

namespace Spells.Data.Thorse
{
    public class Neighty : PassiveController
    {
        [SerializeField] private int damage;
        
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<DeathGA>(KillEnemyReaction, ReactionTiming.POST);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<DeathGA>(KillEnemyReaction, ReactionTiming.POST);
        }

        private void KillEnemyReaction(DeathGA deathGa)
        {
            if (deathGa.killer == cardController)
            {
                foreach (CardMovement cardMovement in TargetingSystem.instance.RetrieveBoard(TargetType.Enemy))
                {
                    DealDamageGA passiveDamage = new DealDamageGA(cardController.singleButton.spellController.ComputeCurrentDamage(damage), cardController, cardMovement.cardController);
                    ActionSystem.instance.AddReaction(passiveDamage);
                }
            }
        }
    }
}
