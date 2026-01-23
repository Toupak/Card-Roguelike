using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;

namespace Combat.Spells.Data.Forceps
{
    public class ForcepsPassive : PassiveController
    {
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<DeathGA>(OnDeathReaction, ReactionTiming.PRE);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<DeathGA>(OnDeathReaction, ReactionTiming.PRE);
        }

        private void OnDeathReaction(DeathGA deathGa)
        {
            if (deathGa.killer != null && deathGa.killer == cardController && deathGa.target.cardStatus.IsStatusApplied(StatusType.Vengeance))
            {
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.PermanentBonusDamage, 1, cardController, cardController);
                ActionSystem.instance.AddReaction(applyStatusGa);
            }
        }
    }
}
