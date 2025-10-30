using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;

namespace Spells.Data.Canis_Balistic
{
    public class DoggoLoaderSpell : SpellController
    {
        protected override IEnumerator CastSpellOnTarget(SpellData spellData, List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(spellData, targets);
            
            LoadBarkBulletGA loadBarkBulletGa = new LoadBarkBulletGA(targets[0].cardController);
            ActionSystem.instance.Perform(loadBarkBulletGa);
        }
    }
}
