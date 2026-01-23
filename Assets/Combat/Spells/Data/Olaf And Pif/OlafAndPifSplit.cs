using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.Spells.Data.Olaf_And_Pif
{
    public class OlafAndPifSplit : SpellController
    {
        [SerializeField] private CardData olafData;
        [SerializeField] private CardData pifData;

        public override void Setup(CardController controller, SpellData data, SpellButton attacheSpellButton, SpellButton otherSpell)
        {
            base.Setup(controller, data, attacheSpellButton, otherSpell);

            if (CombatLoop.instance != null && CombatLoop.instance.currentTurn != CombatLoop.TurnType.Preparation)
                HasCastedThisTurn = true;
        }
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            int health = cardController.cardHealth.currentHealth;
            int olafHealth = Mathf.FloorToInt(health / 2.0f);
            int pifHealth = Mathf.FloorToInt(health / 2.0f);

            if (health % 2 == 1)
                olafHealth += 1;

            SpawnCardGA spawnOlaf = new SpawnCardGA(olafData, cardController);
            spawnOlaf.startingHealth = olafHealth;
            spawnOlaf.frameData = cardController.frameDisplay.hasFrame ? cardController.frameDisplay.data : null;
            ActionSystem.instance.Perform(spawnOlaf);
            
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            
            SpawnCardGA spawnPif = new SpawnCardGA(pifData, cardController);
            spawnPif.startingHealth = pifHealth;
            spawnPif.frameData = cardController.frameDisplay.hasFrame ? cardController.frameDisplay.data : null;
            ActionSystem.instance.Perform(spawnPif, () => cardController.KillCard(false));
        }
    }
}
