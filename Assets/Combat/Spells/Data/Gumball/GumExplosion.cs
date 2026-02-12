using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;
using Combat.Spells.Targeting;
using Combat.Status;

namespace Combat.Spells.Data.Gumball
{
    public class GumExplosion : PassiveController
    {
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<ApplyStatusGa>(ApplyStackReaction, ReactionTiming.POST);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<ApplyStatusGa>(ApplyStackReaction, ReactionTiming.POST);
        }
        
        private void ApplyStackReaction(ApplyStatusGa applyStatusGa)
        {
            if (applyStatusGa.target == cardController && applyStatusGa.type == StatusType.GumBoom && cardController.cardStatus.IsStatusApplied(StatusType.GumBoom))
            {
                int currentStacks = cardController.cardStatus.GetCurrentStackCount(StatusType.GumBoom);
                int maxStacks = StatusSystem.instance.GetStatusData(StatusType.GumBoom).maxStackCount;

                if (currentStacks >= maxStacks)
                {
                    int damages = cardController.cardStatus.GetCurrentStackCount(StatusType.Stun);

                    if (damages > 0)
                    {
                        List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Enemy);
                        foreach (CardMovement target in targets)
                        {
                            DealDamageGA dealDamageGa = new DealDamageGA(damages, cardController, target.cardController);
                            ActionSystem.instance.AddReaction(dealDamageGa);
                        }
                    }
                    
                    DeathGA deathGa = new DeathGA(applyStatusGa.attacker, cardController);
                    ActionSystem.instance.AddReaction(deathGa);
                }
            }
        }
    }
}
