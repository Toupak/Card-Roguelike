using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Passives;
using UnityEngine;

namespace Spells.Data.Smoking_Devil
{
    public class PermanentBonusOnDeath : PassiveController
    {
        [SerializeField] private int damageBuffPerDeath;
        
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<DeathGA>(AngerReaction, ReactionTiming.POST);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<DeathGA>(AngerReaction, ReactionTiming.POST);
        }
        
        private void AngerReaction(DeathGA gA)
        {
            ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.PermanentBonusDamage, damageBuffPerDeath, cardController, cardController);
            ActionSystem.instance.AddReaction(applyStatusGa);
        }
    }
}
