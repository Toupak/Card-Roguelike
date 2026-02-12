using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;
using UnityEngine;

namespace Combat.Spells.Data.Manta
{
    public class MantaPassive : PassiveController
    {
        [SerializeField] private CardData firstTorpedo;
        [SerializeField] private CardData secondTorpedo;
        [SerializeField] private Sprite mantaWithOneTorpedo;
        [SerializeField] private Sprite mantaWithNoTorpedo;
        
        public int torpedoCount { get; private set; } = 2;
        
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.PRE);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.PRE);
        }

        private void DealDamageReaction(DealDamageGA dealDamageGa)
        {
            if (torpedoCount > 0 && dealDamageGa.attacker != null && dealDamageGa.attacker == cardController)
            {
                foreach (DealDamageGA.DamagePackage package in dealDamageGa.packages)
                {
                    if (!package.isDamageNegated && package.amount > 0)
                    {
                        torpedoCount -= 1;
                        SpawnTorpedo();
                        UpdateArtwork();
                        break;
                    }
                }
            }
        }

        private void SpawnTorpedo()
        {
            SpawnCardGA spawnCardGa = new SpawnCardGA(torpedoCount == 1 ? firstTorpedo : secondTorpedo, cardController);
            ActionSystem.instance.AddReaction(spawnCardGa);
        }
        
        private void UpdateArtwork()
        {
            cardController.SetArtwork(torpedoCount == 1 ? mantaWithOneTorpedo : mantaWithNoTorpedo);
        }
        
        public void ReloadTorpedoes()
        {
            torpedoCount = 2;
        }
    }
}
