using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Spells.Targeting;
using UnityEngine;

namespace Spells.Data.Olaf_And_Pif
{
    public class PifRegroup : SpellController
    {
        [SerializeField] private CardData olafAndPifData;
        [SerializeField] private CardData olafData;

        private int totalHealth;
        private CardController olaf;
        private CardController Olaf => olaf == null ? FindOlaf() : olaf;
        
        public override bool CanCastSpell()
        {
            return base.CanCastSpell() && Olaf != null && !Olaf.cardHealth.IsDead;
        }
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            int pifHealth = cardController.cardHealth.currentHealth;
            int olafHealth = Olaf.cardHealth.currentHealth;
            totalHealth = pifHealth + olafHealth;

            SpawnCardGA spawnOlafAndPif = new SpawnCardGA(olafAndPifData, cardController);
            ActionSystem.instance.Perform(spawnOlafAndPif, () =>
            {
                cardController.KillCard(false);
                Olaf.KillCard(false);
            });
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
                spawnCardGa.spawnedCard.cardHealth.SetHealth(totalHealth);
        }
    }
}
