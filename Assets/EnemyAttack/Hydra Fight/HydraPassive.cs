using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Passives;
using UnityEngine;

namespace EnemyAttack.Hydra_Fight
{
    public class HydraPassive : PassiveController
    {
        [SerializeField] private CardData neckData;

        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<DeathGA>(DeathReaction, ReactionTiming.PRE);
        }
        
        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<DeathGA>(DeathReaction, ReactionTiming.PRE);
        }

        private void DeathReaction(DeathGA deathGa)
        {
            if (deathGa.target == cardController)
            {
                SpawnCardGA spawnCardGa = new SpawnCardGA(neckData, cardController);
                ActionSystem.instance.AddReaction(spawnCardGa);
            }
        }
    }
}
