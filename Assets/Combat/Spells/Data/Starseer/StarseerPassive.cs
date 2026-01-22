using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;

namespace Combat.Spells.Data.Starseer
{
    public class StarseerPassive : PassiveController
    {
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<ConsumeStacksGa>(ConsumeStatusReaction, ReactionTiming.PRE);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<ConsumeStacksGa>(ConsumeStatusReaction, ReactionTiming.PRE);
        }

        private void ConsumeStatusReaction(ConsumeStacksGa consumeStacksGa)
        {
            if (consumeStacksGa.target == cardController && consumeStacksGa.attacker == cardController && consumeStacksGa.type == StatusType.BonusDamage)
                consumeStacksGa.amount = 0;
        }
    }
}
