using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.EnemyAttack;
using Combat.Passives;
using Combat.Spells;
using Combat.Spells.Targeting;
using Combat.Status;
using System.Collections.Generic;
using UnityEngine;

public class DebuffPassive : PassiveController
{
    [SerializeField] private StatusType statusType;
    [SerializeField] private int stacks;

    [Space]
    [SerializeField] private bool startOfTurn;
    [SerializeField] private bool endOfTurn;

    private void OnEnable()
    {
        if (startOfTurn)
            ActionSystem.SubscribeReaction<StartTurnGa>(ApplyDebuffSoT, ReactionTiming.POST);

        if (endOfTurn)
            ActionSystem.SubscribeReaction<EndTurnGA>(ApplyDebuffEoT, ReactionTiming.POST);
    }

    private void OnDisable()
    {
        if (startOfTurn)
            ActionSystem.UnsubscribeReaction<StartTurnGa>(ApplyDebuffSoT, ReactionTiming.POST);

        if (endOfTurn)
            ActionSystem.UnsubscribeReaction<EndTurnGA>(ApplyDebuffEoT, ReactionTiming.POST);
    }

    private void ApplyDebuffSoT(StartTurnGa gA)
    {
        if (gA.starting == Combat.CombatLoop.TurnType.Enemy)
            ApplyDebuff();
    }

    private void ApplyDebuffEoT(EndTurnGA gA)
    {
        if (gA.ending == Combat.CombatLoop.TurnType.Enemy)
            ApplyDebuff();
    }

    private void ApplyDebuff()
    {
        List<CardMovement> cards = TargetingSystem.instance.RetrieveBoard(TargetType.Ally);
        CardController target = PickRandomTarget(cards);

        ApplyStatusGa applyStatusGa = new ApplyStatusGa(statusType, stacks, cardController, target);
        ActionSystem.instance.AddReaction(applyStatusGa);
    }

    //Unity Editor - so both bools EoT / SoT can't be checked at the same time
    private bool previousStartOfTurn;
    private bool previousEndOfTurn;
    private void OnValidate()
    {
        if (startOfTurn != previousStartOfTurn)
        {
            if (startOfTurn)
                endOfTurn = false;
        }
        else if (endOfTurn != previousEndOfTurn)
        {
            if (endOfTurn)
                startOfTurn = false;
        }

        previousStartOfTurn = startOfTurn;
        previousEndOfTurn = endOfTurn;
    }
}