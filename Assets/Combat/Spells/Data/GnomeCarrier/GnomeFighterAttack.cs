using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;
using Combat.Spells.Targeting;

namespace Combat.Spells.Data.GnomeCarrier
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
            if (startTurnGa.starting != CombatLoop.TurnType.Player)
                return;

            CardController target = PickRandomTarget(TargetingSystem.instance.RetrieveBoard(TargetType.Enemy));
            if (target != null)
            {
                DealDamageGA dealDamageGa = new DealDamageGA(cardController.ComputeCurrentDamage(1), cardController, target);
                ActionSystem.instance.AddReaction(dealDamageGa);
            }

            DeathGA deathGa = new DeathGA(cardController, cardController);
            ActionSystem.instance.AddReaction(deathGa);
        }
    }
}
