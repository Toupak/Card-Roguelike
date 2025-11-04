using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Spells.Targeting;

namespace Spells.Data.Hog
{
    public class BoarSploader : SpellController
    {
        protected override void SubscribeReactions()
        {
            ActionSystem.SubscribeReaction<ConsumeStacksGa>(ConsumeStacksReaction, ReactionTiming.POST);
            ActionSystem.SubscribeReaction<DeathGA>(ExplodeOnDeathReaction, ReactionTiming.PRE);
        }

        protected override  void UnsubscribeReactions()
        {
            ActionSystem.UnsubscribeReaction<ConsumeStacksGa>(ConsumeStacksReaction, ReactionTiming.POST);
            ActionSystem.UnsubscribeReaction<DeathGA>(ExplodeOnDeathReaction, ReactionTiming.PRE);
        }
        
        private void ConsumeStacksReaction(ConsumeStacksGa consumeStacksGa)
        {
            if (consumeStacksGa.target == cardController && consumeStacksGa.type == StatusType.Taunt)
            {
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.HogGroink, 1, cardController, cardController);
                ActionSystem.instance.AddReaction(applyStatusGa);
            }
        }
        
        protected override IEnumerator CastSpellOnTarget(SpellData spellData, List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(spellData, targets);
            
            ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.HogGroink, 3, cardController, cardController);
            ActionSystem.instance.Perform(applyStatusGa);
        }
        
        private void ExplodeOnDeathReaction(DeathGA deathGa)
        {
            if (deathGa.target == cardController)
            {
                int damage = cardController.cardStatus.currentStacks.TryGetValue(StatusType.HogGroink, out var stack) ? stack : 0;
                
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
