using System.Collections;
using ActionReaction;
using Cards.Scripts;
using UnityEngine;

namespace Spells.Data.Canis_Balistic
{
    public class DoggoLoaderSpell : SpellController
    {
        private void OnEnable()
        {
            ActionSystem.AttachPerformer<LoadBarkBulletGA>(LoadBarkBulletPerformer);
        }

        private void OnDisable()
        {
            ActionSystem.DetachPerformer<LoadBarkBulletGA>();
        }

        private IEnumerator LoadBarkBulletPerformer(LoadBarkBulletGA loadBarkBulletGa)
        {
            yield break;
        }
        
        protected override IEnumerator CastSpellOnSelf(SpellData spellData, CardMovement thisCard)
        {
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            Debug.Log($"Cast Basic Damage Spell {spellData.spellName} on targets : ");
            OnCastSpell?.Invoke();
            
            LoadBarkBulletGA loadBarkBulletGa = new LoadBarkBulletGA(thisCard.cardController);
            ActionSystem.instance.Perform(loadBarkBulletGa);
        }
    }
}
