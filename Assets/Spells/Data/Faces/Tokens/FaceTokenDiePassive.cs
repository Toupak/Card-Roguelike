using ActionReaction;
using ActionReaction.Game_Actions;
using Passives;

namespace Spells.Data.Faces.Tokens
{
    public class FaceTokenDiePassive : PassiveController
    {
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
            if (startTurnGa.starting == CombatLoop.CombatLoop.TurnType.Player)
            {
                DealDamageGA dealDamageGa = new DealDamageGA(1, cardController, cardController);
                ActionSystem.instance.AddReaction(dealDamageGa);
            }
        }
    }
}
