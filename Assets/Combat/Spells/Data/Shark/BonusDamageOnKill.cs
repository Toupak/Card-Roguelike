using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;

namespace Combat.Spells.Data.Shark
{
    public class BonusDamageOnKill : PassiveController
    {
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<DeathGA>(KillReaction, ReactionTiming.POST);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<DeathGA>(KillReaction, ReactionTiming.POST);
        }

        private void KillReaction(DeathGA deathGa)
        {
            if (deathGa.killer == cardController)
            {
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.PermanentBonusDamage, 1, cardController, cardController);
                ActionSystem.instance.AddReaction(applyStatusGa);
            }
        }
    }
}
