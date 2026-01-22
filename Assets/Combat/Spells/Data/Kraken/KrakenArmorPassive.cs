using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;
using Combat.Spells.Targeting;

namespace Combat.Spells.Data.Kraken
{
    public class KrakenArmorPassive : PassiveController
    {
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.POST);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.POST);
        }

        private void DealDamageReaction(DealDamageGA dealDamageGa)
        {
            if (dealDamageGa.attacker == cardController)
            {
                int currentSlotIndex = cardController.cardMovement.SlotIndex;

                List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(cardController.cardMovement.IsEnemyCard ? TargetType.Enemy : TargetType.Ally, true);

                for (int i = currentSlotIndex - 1; i >= 0; i--)
                {
                    if (targets[i].cardController.IsTargetable())
                    {
                        ApplyStatusGa spread = new ApplyStatusGa(StatusType.Armor, 1, cardController, targets[i].cardController);
                        ActionSystem.instance.AddReaction(spread);
                        break;
                    }
                }

                for (int i = currentSlotIndex + 1; i < targets.Count; i++)
                {
                    if (targets[i].cardController.IsTargetable())
                    {
                        ApplyStatusGa spread = new ApplyStatusGa(StatusType.Armor, 1, cardController, targets[i].cardController);
                        ActionSystem.instance.AddReaction(spread);
                        break;
                    }
                }
            }
        }
    }
}
