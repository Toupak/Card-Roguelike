using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;
using Combat.Spells.Targeting;

namespace Combat.Spells.Data.Manta
{
    public class TorpedoPassive : PassiveController
    {
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<DeathGA>(DeathReaction, ReactionTiming.PRE);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<DeathGA>(DeathReaction, ReactionTiming.PRE);
        }

        private void DeathReaction(DeathGA deathGa)
        {
            if (deathGa.target == cardController)
            {
                List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Enemy);

                foreach (CardMovement cardMovement in targets)
                {
                    ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Weak, 1, cardController, cardMovement.cardController);
                    ActionSystem.instance.AddReaction(applyStatusGa);
                }
            }
        }
    }
}
