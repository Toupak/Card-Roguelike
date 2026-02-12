using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;
using UnityEngine;

namespace Combat.Spells.Data.Dino_And_Zaur
{
    public class DinoAndZaurPassive : PassiveController
    {
        [SerializeField] private CardData zaurData;
        
        [Space]
        [SerializeField] private SpellData duoSpell;
        [SerializeField] private SpellData soloSpell;

        [Space] 
        [SerializeField] private Sprite dinoAndZaurSprite;
        [SerializeField] private Sprite dinoSoloSprite;

        [Space] 
        [SerializeField] private string cardNameWhenDismounted;
        
        private bool isMounted = true;
        
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<DealDamageGA>(PreReaction, ReactionTiming.PRE);
            ActionSystem.SubscribeReaction<DealDamageGA>(PostReaction, ReactionTiming.POST);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<DealDamageGA>(PreReaction, ReactionTiming.PRE);
            ActionSystem.UnsubscribeReaction<DealDamageGA>(PostReaction, ReactionTiming.POST);
        }

        private void PreReaction(DealDamageGA dealDamageGa)
        {
            DealDamageGA.DamagePackage package = dealDamageGa.GetDamagePackageForTarget(cardController);
            
            if (isMounted && package != null)
                dealDamageGa.NegateDamage(package);
        }

        private void PostReaction(DealDamageGA dealDamageGa)
        {
            if (isMounted && dealDamageGa.IsCardTargeted(cardController))
                Dismount();
        }

        private void Dismount()
        {
            isMounted = false;
            
            SpawnCardGA spawnCardGa = new SpawnCardGA(zaurData, cardController);
            ActionSystem.instance.AddReaction(spawnCardGa);

            cardController.SetupSingleButton(soloSpell);
            cardController.SetArtwork(dinoSoloSprite);
            cardController.SetCardName(cardNameWhenDismounted);
        }

        public void Remount()
        {
            isMounted = true;
            
            cardController.SetupSingleButton(duoSpell);
            cardController.SetArtwork(dinoAndZaurSprite);
            cardController.SetCardName(cardController.cardData.cardName);
        }
    }
}
