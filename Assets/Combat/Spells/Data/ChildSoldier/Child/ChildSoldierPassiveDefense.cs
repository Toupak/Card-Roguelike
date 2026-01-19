using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Passives;
using UnityEngine;

namespace Spells.Data.ChildSoldier.Child
{
    public class ChildSoldierPassiveDefense : PassiveController
    {
        [SerializeField] private CardData teenDefenseData;
        [SerializeField] private int stacksRequiredBeforeEvolution;
        
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.POST);
            ActionSystem.SubscribeReaction<ApplyStatusGa>(ApplyStatusReaction, ReactionTiming.POST);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.POST);
            ActionSystem.UnsubscribeReaction<ApplyStatusGa>(ApplyStatusReaction, ReactionTiming.POST);
        }

        private void DealDamageReaction(DealDamageGA dealDamageGa)
        {
            if (dealDamageGa.target == cardController)
            {
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Blood, dealDamageGa.amount, cardController, cardController);
                ActionSystem.instance.AddReaction(applyStatusGa);
            }
        }
        
        private void ApplyStatusReaction(ApplyStatusGa applyStatusGa)
        {
            if (applyStatusGa.target == cardController && applyStatusGa.type == StatusType.Blood)
            {
                if (cardController.cardStatus.GetCurrentStackCount(StatusType.Blood) >= stacksRequiredBeforeEvolution)
                    EvolveIntoTeen();
            }
        }

        private void EvolveIntoTeen()
        {
            SpawnCardGA spawnCardGa = new SpawnCardGA(teenDefenseData, cardController);
            spawnCardGa.startingHealth = cardController.cardHealth.currentHealth;
            spawnCardGa.deckCard = cardController.deckCard;
            ActionSystem.instance.AddReaction(spawnCardGa);
            
            cardController.KillCard(false);
        }
    }
}
