using System;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Passives;

namespace Spells.Data.Cube
{
    public class CubePassive : PassiveController
    {
        public void OnEnable()
        {
            ActionSystem.SubscribeReaction<SpawnCardGA>(SpawnCardReaction, ReactionTiming.POST);
        }

        public void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<SpawnCardGA>(SpawnCardReaction, ReactionTiming.POST);
        }

        private void SpawnCardReaction(SpawnCardGA spawnCardGa)
        {
            if (!spawnCardGa.spawner.cardMovement.IsEnemyCard && spawnCardGa.spawnedCard != null)
            {
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.FreeSpell, 1, cardController, spawnCardGa.spawnedCard);
                ActionSystem.instance.AddReaction(applyStatusGa);
            }
        }
    }
}
