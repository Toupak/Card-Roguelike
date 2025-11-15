using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Passives;

namespace Spells.Data.Barbarian
{
    public class BerserkBuff : PassiveController
    {
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<ConsumeStacksGa>(ConsumeRageReaction, ReactionTiming.POST, 50);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<ConsumeStacksGa>(ConsumeRageReaction, ReactionTiming.POST);
        }

        private void ConsumeRageReaction(ConsumeStacksGa consumeStacksGa)
        {
            if (consumeStacksGa.target == cardController && consumeStacksGa.type == StatusType.Rage)
            {
                int weakStacks = cardController.cardStatus.GetCurrentStackCount(StatusType.Weak);
                if (weakStacks > 0)
                {
                    ConsumeStacksGa consumeWeak = new ConsumeStacksGa(StatusType.Weak, weakStacks, cardController, cardController);
                    ActionSystem.instance.AddReaction(consumeWeak);
                }
                
                int stunStacks = cardController.cardStatus.GetCurrentStackCount(StatusType.Stun);
                if (stunStacks > 0)
                {
                    ConsumeStacksGa consumeStun = new ConsumeStacksGa(StatusType.Stun, stunStacks, cardController, cardController);
                    ActionSystem.instance.AddReaction(consumeStun);
                }

                ApplyStatusGa goBerserk = new ApplyStatusGa(StatusType.PermanentBonusDamage, 1, cardController, cardController);
                ActionSystem.instance.AddReaction(goBerserk);
            }
        }
    }
}
