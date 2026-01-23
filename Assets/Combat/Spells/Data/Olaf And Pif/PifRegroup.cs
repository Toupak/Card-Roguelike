using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Spells.Targeting;
using UnityEngine;

namespace Combat.Spells.Data.Olaf_And_Pif
{
    public class PifRegroup : SpellController
    {
        [SerializeField] private CardData olafAndPifData;
        [SerializeField] private CardData olafData;

        private Dictionary<StatusType, int> totalStacks = new Dictionary<StatusType, int>();
        private CardController olaf;
        private CardController Olaf => olaf == null ? FindOlaf() : olaf;
        
        public override bool CanCastSpell()
        {
            return base.CanCastSpell() && Olaf != null && !Olaf.cardHealth.IsDead;
        }
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            
            int totalHealth = ComputeTotalHealth();
            totalStacks = ComputeTotalStacks();

            SpawnCardGA spawnOlafAndPif = new SpawnCardGA(olafAndPifData, cardController);
            spawnOlafAndPif.startingHealth = totalHealth;
            spawnOlafAndPif.frameData = cardController.frameDisplay.hasFrame ? cardController.frameDisplay.data : null;
            ActionSystem.instance.Perform(spawnOlafAndPif, () =>
            {
                cardController.KillCard(false);
                Olaf.KillCard(false);
            });
        }

        protected override void SubscribeReactions()
        {
            base.SubscribeReactions();
            ActionSystem.SubscribeReaction<SpawnCardGA>(SpawnCardReaction, ReactionTiming.POST);
        }

        protected override void UnsubscribeReactions()
        {
            base.UnsubscribeReactions();
            ActionSystem.UnsubscribeReaction<SpawnCardGA>(SpawnCardReaction, ReactionTiming.POST);
        }

        private void SpawnCardReaction(SpawnCardGA spawnCardGa)
        {
            if (spawnCardGa.cardData.name == olafAndPifData.name)
            {
                ApplyTotalStacks(spawnCardGa.spawnedCard.cardStatus);
            }
        }

        private void ApplyTotalStacks(CardStatus spawnedCardStatus)
        {
            foreach (KeyValuePair<StatusType,int> pair in totalStacks)
            {
                spawnedCardStatus.ApplyStatusStacks(pair.Key, pair.Value);
            }
        }

        private int ComputeTotalHealth()
        {
            int pifHealth = cardController.cardHealth.currentHealth;
            int olafHealth = Olaf.cardHealth.currentHealth;

            return pifHealth + olafHealth;
        }

        private Dictionary<StatusType,int> ComputeTotalStacks()
        {
            Dictionary<StatusType, int> stacks = new Dictionary<StatusType, int>(cardController.cardStatus.currentStacks);

            foreach (KeyValuePair<StatusType,int> pair in Olaf.cardStatus.currentStacks)
            {
                if (stacks.ContainsKey(pair.Key))
                    stacks[pair.Key] += pair.Value;
                else
                    stacks[pair.Key] = pair.Value;
            }

            return stacks;
        }
        
        private CardController FindOlaf()
        {
            List<CardMovement> cards = TargetingSystem.instance.RetrieveBoard(TargetType.Ally);

            foreach (CardMovement card in cards)
            {
                if (card.cardController != null && card.cardController.cardData.name == olafData.name)
                {
                    olaf = card.cardController;
                    return olaf;
                }
            }

            return null;
        }
    }
}
