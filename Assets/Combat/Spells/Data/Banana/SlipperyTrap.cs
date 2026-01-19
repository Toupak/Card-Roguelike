using ActionReaction;
using ActionReaction.Game_Actions;
using BoomLib.Tools;
using Cards.Scripts;
using Combat.Passives;

namespace Combat.Spells.Data.Banana
{
    public class SlipperyTrap : PassiveController
    {
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<DealDamageGA>(SlipperyTrapReaction, ReactionTiming.POST);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<DealDamageGA>(SlipperyTrapReaction, ReactionTiming.POST);
        }

        private void SlipperyTrapReaction(DealDamageGA dealDamageGa)
        {
            if (dealDamageGa.target == cardController)
            {
                bool random = Tools.RandomBool();

                if (random)
                {
                    dealDamageGa.NegateDamage();
                    ApplyStatusGa stun = new ApplyStatusGa(StatusType.Stun, 2, cardController, dealDamageGa.attacker);
                    ActionSystem.instance.AddReaction(stun);
                }
            }
        }
    }
}
