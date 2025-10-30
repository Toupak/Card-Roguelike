using System.Collections;
using System.Collections.Generic;
using Cards.Scripts;

namespace Spells.Data.Canis_Balistic
{
    public class DoggoLoaderSpell : SpellController
    {
        private BarkSpell barkSpell = null;
        
        protected override IEnumerator CastSpellOnTarget(SpellData spellData, List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(spellData, targets);

            if (barkSpell == null)
                barkSpell = otherSpellButton.GetComponentInChildren<BarkSpell>();
            
            if (barkSpell != null)
                barkSpell.LoadBullet();
        }
    }
}
