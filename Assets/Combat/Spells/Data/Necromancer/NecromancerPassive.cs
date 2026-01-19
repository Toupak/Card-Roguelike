using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;

namespace Combat.Spells.Data.Necromancer
{
    public class NecromancerPassive : PassiveController
    {
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<DeathGA>(DeathReaction, ReactionTiming.POST);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<DeathGA>(DeathReaction, ReactionTiming.POST);
        }

        private void DeathReaction(DeathGA deathGa)
        {
            ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Corpse, 1, cardController, cardController);
            ActionSystem.instance.AddReaction(applyStatusGa);
        }
    }
}
