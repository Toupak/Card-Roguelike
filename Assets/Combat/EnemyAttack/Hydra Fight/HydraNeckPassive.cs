using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;
using UnityEngine;

namespace Combat.EnemyAttack.Hydra_Fight
{
    public class HydraNeckPassive : PassiveController
    {
        [SerializeField] private CardData headData;
        [SerializeField] private int turnsBeforeHeadRegrow;

        private int currentTurn;
        
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
            if (startTurnGa.starting == CombatLoop.TurnType.Enemy)
            {
                currentTurn += 1;

                if (currentTurn >= turnsBeforeHeadRegrow)
                {
                    DeathGA deathGa = new DeathGA(cardController, cardController);
                    ActionSystem.instance.AddReaction(deathGa);
                        
                    SpawnCardGA spawnCardGa = new SpawnCardGA(headData, cardController);
                    ActionSystem.instance.AddReaction(spawnCardGa);
                }
            }
        }
    }
}
