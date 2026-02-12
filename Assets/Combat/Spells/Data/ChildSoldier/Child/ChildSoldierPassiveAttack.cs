using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;
using UnityEngine;

namespace Combat.Spells.Data.ChildSoldier.Child
{
    public class ChildSoldierPassiveAttack : PassiveController
    {
        [SerializeField] private CardData teenAttackData;
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
            if (dealDamageGa.attacker == cardController)
            {
                foreach (DealDamageGA.DamagePackage package in dealDamageGa.packages)
                {
                    ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Vengeance, package.amount, cardController, cardController);
                    ActionSystem.instance.AddReaction(applyStatusGa);
                }
            }
        }
        
        private void ApplyStatusReaction(ApplyStatusGa applyStatusGa)
        {
            if (applyStatusGa.target == cardController && applyStatusGa.type == StatusType.Vengeance)
            {
                if (cardController.cardStatus.GetCurrentStackCount(StatusType.Vengeance) >= stacksRequiredBeforeEvolution)
                    EvolveIntoTeen();
            }
        }

        private void EvolveIntoTeen()
        {
            SpawnCardGA spawnCardGa = new SpawnCardGA(teenAttackData, cardController);
            spawnCardGa.startingHealth = cardController.cardHealth.currentHealth;
            spawnCardGa.frameData = cardController.frameDisplay.hasFrame ? cardController.frameDisplay.data : null;
            
            cardController.deckCard.cardData = teenAttackData;
            spawnCardGa.deckCard = cardController.deckCard;
            ActionSystem.instance.AddReaction(spawnCardGa);
            
            cardController.KillCard(false);
        }
    }
}
