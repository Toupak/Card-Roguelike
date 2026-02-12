using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;
using Combat.Spells.Targeting;

namespace Combat.Spells.Data.Hog
{
    public class BoarSploader : PassiveController
    {
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<ConsumeStacksGa>(ConsumeStacksReaction, ReactionTiming.POST);
            ActionSystem.SubscribeReaction<DeathGA>(ExplodeOnDeathReaction, ReactionTiming.PRE);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<ConsumeStacksGa>(ConsumeStacksReaction, ReactionTiming.POST);
            ActionSystem.UnsubscribeReaction<DeathGA>(ExplodeOnDeathReaction, ReactionTiming.PRE);
        }
        
        private void ConsumeStacksReaction(ConsumeStacksGa consumeStacksGa)
        {
            if (consumeStacksGa.target == cardController && consumeStacksGa.type == StatusType.Taunt && consumeStacksGa.wasConsumed)
            {
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.HogGroink, 1, cardController, cardController);
                ActionSystem.instance.AddReaction(applyStatusGa);
            }
        }

        private void ExplodeOnDeathReaction(DeathGA deathGa)
        {
            if (deathGa.target == cardController)
            {
                int damage = cardController.cardStatus.GetCurrentStackCount(StatusType.HogGroink);
                
                List<CardMovement> enemies = TargetingSystem.instance.RetrieveBoard(TargetType.Enemy);

                foreach (CardMovement enemy in enemies)
                {
                    DealDamageGA dealDamage = new DealDamageGA(damage, cardController, enemy.cardController);
                    ActionSystem.instance.AddReaction(dealDamage);
                }
            }
        }
    }
}
