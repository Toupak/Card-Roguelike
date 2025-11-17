using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Spells.Data.Olaf_And_Pif
{
    public class OlafAndPifSplit : SpellController
    {
        [SerializeField] private CardData olafData;
        [SerializeField] private CardData pifData;

        private int olafHealth;
        private int pifHealth;
        
        public override void Setup(CardController controller, SpellData data, SpellButton attacheSpellButton, SpellButton otherSpell)
        {
            base.Setup(controller, data, attacheSpellButton, otherSpell);

            if (CombatLoop.CombatLoop.instance.currentTurn != CombatLoop.CombatLoop.TurnType.Preparation)
                HasCastedThisTurn = true;
        }
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            int health = cardController.cardHealth.currentHealth;
            olafHealth = Mathf.FloorToInt(health / 2.0f);
            pifHealth = Mathf.FloorToInt(health / 2.0f);

            if (health % 2 == 1)
                olafHealth += 1;

            SpawnCardGA spawnOlaf = new SpawnCardGA(olafData, cardController);
            ActionSystem.instance.Perform(spawnOlaf);
            
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            
            SpawnCardGA spawnPif = new SpawnCardGA(pifData, cardController);
            ActionSystem.instance.Perform(spawnPif, () => cardController.KillCard(false));
        }
        
        protected override void SubscribeReactions()
        {
            base.SubscribeReactions();
            ActionSystem.SubscribeReaction<SpawnCardGA>(SpawnCardReaction, ReactionTiming.POST, 150);
        }

        protected override void UnsubscribeReactions()
        {
            base.UnsubscribeReactions();
            ActionSystem.UnsubscribeReaction<SpawnCardGA>(SpawnCardReaction, ReactionTiming.POST);
        }

        private void SpawnCardReaction(SpawnCardGA spawnCardGa)
        {
            if (spawnCardGa.cardData == olafData)
                spawnCardGa.spawnedCard.cardHealth.SetHealth(olafHealth);
            else if (spawnCardGa.cardData == pifData)
                spawnCardGa.spawnedCard.cardHealth.SetHealth(pifHealth);
        }
    }
}
