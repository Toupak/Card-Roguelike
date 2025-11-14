using ActionReaction;
using ActionReaction.Game_Actions;
using Passives;
using Spells.Targeting;

namespace Spells.Data.GnomeCarrier
{
    public class GnomeFighterAttack : PassiveController
    {
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<StartTurnGa>(StartTurnReaction, ReactionTiming.PRE);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<StartTurnGa>(StartTurnReaction, ReactionTiming.PRE);
        }

        private void StartTurnReaction(StartTurnGa startTurnGa)
        {
            if (startTurnGa.starting != CombatLoop.CombatLoop.TurnType.Player)
                return;
            
            DealDamageGA dealDamageGa = new DealDamageGA(1, cardController, PickRandomTarget(TargetingSystem.instance.RetrieveBoard(TargetType.Enemy)));
            ActionSystem.instance.AddReaction(dealDamageGa);

            DeathGA deathGa = new DeathGA(cardController, cardController);
            ActionSystem.instance.AddReaction(deathGa);
        }
    }
}
