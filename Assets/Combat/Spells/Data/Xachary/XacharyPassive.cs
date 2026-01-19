using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;

namespace Combat.Spells.Data.Xachary
{
    public class XacharyPassive : PassiveController
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
            if (startTurnGa.starting == CombatLoop.TurnType.Player)
            {
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.BulletAmmo, 1, cardController, cardController);
                ActionSystem.instance.AddReaction(applyStatusGa);
            }
        }
    }
}
