using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Passives;
using UnityEngine;

namespace Spells.Data.Snuk
{
    public class SnukEggPassive : PassiveController
    {
        [SerializeField] private CardData tokenData;

        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<StartTurnGa>(StartTurnReaction, ReactionTiming.POST);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<StartTurnGa>(StartTurnReaction, ReactionTiming.POST);
        }

        private void StartTurnReaction(StartTurnGa startTurnGa)
        {
            if (startTurnGa.starting == CombatLoop.CombatLoop.TurnType.Player)
            {
                SpawnCardGA spawnCardGa = new SpawnCardGA(tokenData, cardController, true);
                ActionSystem.instance.AddReaction(spawnCardGa);
            }
        }
    }
}
