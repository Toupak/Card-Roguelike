using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;

namespace Combat.Spells.Data.Puppeteer.Puppet
{
    public class PuppetDefensiveEnemy : PassiveController
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
            if (startTurnGa.starting == CombatLoop.TurnType.Enemy)
            {
                ApplyStatusGa applyStatus = new ApplyStatusGa(StatusType.Weak, 1, cardController, cardController.tokenParentController);
                ActionSystem.instance.AddReaction(applyStatus);
            }
        }
    }
}
