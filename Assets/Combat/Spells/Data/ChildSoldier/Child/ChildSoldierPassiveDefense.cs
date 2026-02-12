using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;
using UnityEngine;

namespace Combat.Spells.Data.ChildSoldier.Child
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
            DealDamageGA.DamagePackage damagePackage = dealDamageGa.GetPackageFromTarget(cardController);
            
            if (damagePackage != null)
            {
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Blood, damagePackage.amount, cardController, cardController);
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
            spawnCardGa.frameData = cardController.frameDisplay.hasFrame ? cardController.frameDisplay.data : null;
            
            cardController.deckCard.cardData = teenDefenseData;
            spawnCardGa.deckCard = cardController.deckCard;
            ActionSystem.instance.AddReaction(spawnCardGa);
            
            cardController.KillCard(false);
        }
    }
}
