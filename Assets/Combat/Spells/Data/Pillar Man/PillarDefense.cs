using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;

namespace Combat.Spells.Data.Pillar_Man
{
    public class PillarDefense : PassiveController
    {
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.PRE);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.PRE);
        }
        
        private void DealDamageReaction(DealDamageGA dealDamageGa)
        {
            if (dealDamageGa.target == cardController)
            {
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Weak, 1, dealDamageGa.attacker, dealDamageGa.attacker);
                ActionSystem.instance.AddReaction(applyStatusGa);
            }
        }
    }
}
