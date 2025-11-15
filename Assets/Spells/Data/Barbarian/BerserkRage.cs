using System;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Passives;
using Spells.Targeting;
using UnityEngine;

namespace Spells.Data.Barbarian
{
    public class BerserkRage : PassiveController
    {
        [SerializeField] private int stackCountRequiredToGoBerserk;
        [SerializeField] private int turnCountAsBerserk;
        
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<DealDamageGA>(DamageReaction, ReactionTiming.POST, 50);
            ActionSystem.SubscribeReaction<ApplyStatusGa>(ApplyStatusReaction, ReactionTiming.POST);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<DealDamageGA>(DamageReaction, ReactionTiming.POST);
            ActionSystem.UnsubscribeReaction<ApplyStatusGa>(ApplyStatusReaction, ReactionTiming.POST);
        }

        private void DamageReaction(DealDamageGA dealDamageGa)
        {
            if (dealDamageGa.attacker == cardController || dealDamageGa.target == cardController)
            {
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Rage, dealDamageGa.amount, cardController, cardController);
                ActionSystem.instance.AddReaction(applyStatusGa);
            }
        }

        private void ApplyStatusReaction(ApplyStatusGa applyStatusGa)
        {
            if (applyStatusGa.target == cardController && applyStatusGa.type == StatusType.Rage)
            {
                int rageStackCount = cardController.cardStatus.GetCurrentStackCount(StatusType.Rage);
                
                if (rageStackCount >= stackCountRequiredToGoBerserk)
                {
                    ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.Rage, rageStackCount, cardController, cardController);
                    ActionSystem.instance.AddReaction(consumeStacksGa);

                    ApplyStatusGa goBerserk = new ApplyStatusGa(StatusType.BerserkMode, turnCountAsBerserk, cardController, cardController);
                    ActionSystem.instance.AddReaction(goBerserk);

                    List<CardMovement> enemies = TargetingSystem.instance.RetrieveBoard(TargetType.Enemy);
                    foreach (CardMovement enemy in enemies)
                    {
                        DealDamageGA dealDamageGa = new DealDamageGA(1, cardController, enemy.cardController);
                        ActionSystem.instance.AddReaction(dealDamageGa);
                    }
                }
            }   
        }
    }
}
