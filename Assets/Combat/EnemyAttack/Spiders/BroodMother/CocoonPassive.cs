using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;
using UnityEngine;

public class CocoonPassive : PassiveController
{
    [SerializeField] private int turnCountBeforePhase2;
    [SerializeField] CardData broodMotherCardData;

    private int turnCount;

    private void OnEnable()
    {
        ActionSystem.SubscribeReaction<StartTurnGa>(TryTriggerPhase2, ReactionTiming.PRE);
    }

    private void OnDisable()
    {
        ActionSystem.UnsubscribeReaction<StartTurnGa>(TryTriggerPhase2, ReactionTiming.PRE);
    }

    private void TryTriggerPhase2(StartTurnGa turn)
    {
        if (turn.starting == Combat.CombatLoop.TurnType.Enemy)
            turnCount += 1;

        if (turnCount >= turnCountBeforePhase2)
        {
            SpawnCardGA spawnCocon = new SpawnCardGA(broodMotherCardData, cardController);
            ActionSystem.instance.AddReaction(spawnCocon);

            cardController.KillCard();
        }
    }
}
