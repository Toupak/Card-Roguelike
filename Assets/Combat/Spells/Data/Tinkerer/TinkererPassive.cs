using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;
using Combat.Spells.Targeting;
using UnityEngine;

namespace Combat.Spells.Data.Tinkerer
{
    public class TinkererPassive : PassiveController
    {
        [SerializeField] private CardData powerCellData;
        
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<SpawnCardGA>(SpawnCardReaction, ReactionTiming.POST);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<SpawnCardGA>(SpawnCardReaction, ReactionTiming.POST);
        }

        private void SpawnCardReaction(SpawnCardGA spawnCardGa)
        {
            if (spawnCardGa.spawner == cardController && spawnCardGa.isToken)
            {
                if (cardController.cardMovement.tokenContainer.Slots.Count > 2)
                {
                    int powerCellCount = CountAndRemoveTrinkets();
                    TriggerTrinketPower(powerCellCount);
                }
            }
        }

        private void TriggerTrinketPower(int powerCellCount)
        {
            if (powerCellCount == 3)
                TriggerEnergy();
            else if (powerCellCount == 2)
                TriggerArmor();
            else if (powerCellCount == 1)
                TriggerWeak();
            else if (powerCellCount == 0)
                TriggerDamage();
        }

        private void TriggerEnergy()
        {
            GainEnergyGa gainEnergyGa = new GainEnergyGa(1, cardController);
            ActionSystem.instance.AddReaction(gainEnergyGa);
        }

        private void TriggerArmor()
        {
            List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Ally);

            foreach (CardMovement target in targets)
            {
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Armor, 1, cardController, target.cardController);
                ActionSystem.instance.AddReaction(applyStatusGa);
            }
        }

        private void TriggerWeak()
        {
            List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Enemy);

            foreach (CardMovement target in targets)
            {
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Weak, 1, cardController, target.cardController);
                ActionSystem.instance.AddReaction(applyStatusGa);
            }
        }

        private void TriggerDamage()
        {
            List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Ally);

            foreach (CardMovement target in targets)
            {
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.BonusDamage, 1, cardController, target.cardController);
                ActionSystem.instance.AddReaction(applyStatusGa);
            }
        }

        private int CountAndRemoveTrinkets()
        {
            int powerCellCount = 0;
            
            int count = cardController.cardMovement.tokenContainer.Slots.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                CardController target = cardController.cardMovement.tokenContainer.Slots[i].CurrentCard.cardController;

                if (target.cardData.cardName == powerCellData.cardName)
                    powerCellCount += 1;

                DeathGA deathGa = new DeathGA(cardController, target);
                ActionSystem.instance.AddReaction(deathGa);
            }

            return powerCellCount;
        }
    }
}
