using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.Spells.Data.Archemologist
{
    public class MoleDig : SpellController
    {
        [SerializeField] private List<CardData> artefacts;
        
        public override bool CanCastSpell()
        {
            return base.CanCastSpell() && cardController.cardMovement.tokenContainer.slotCount < 3;
        }
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            CardData artefact = PickRandomArtefact();
            SpawnCardGA spawnCardGa = new SpawnCardGA(artefact, cardController, true);
            ActionSystem.instance.Perform(spawnCardGa);
        }

        private CardData PickRandomArtefact()
        {
            CardData.Rarity randomRarity = SelectRandomRarity();
            List<CardData> validArtefact = artefacts.Where((a) => a.rarity == randomRarity).ToList();
            
            return validArtefact[Random.Range(0, validArtefact.Count)];
        }

        private CardData.Rarity SelectRandomRarity()
        {
            int random = Random.Range(0, 100);

            if (random < 40)
                return CardData.Rarity.Common;
            if (random < 70)
                return CardData.Rarity.Rare;
            if (random < 90)
                return CardData.Rarity.Legendary;
            
            return CardData.Rarity.Exotic;
        }
    }
}
