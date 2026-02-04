using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.EnemyAttack;
using Combat.Passives;
using Combat.Spells.Targeting;
using UnityEngine;


public class ApplyStatusPassive : PassiveController
{
    [SerializeField] StatusType status;
    [SerializeField] int stackAmount;

    [Space]
    [SerializeField] bool multipleTarget;
    [SerializeField] int targetAmount;

    private void OnEnable()
    {
        ActionSystem.SubscribeReaction<StartTurnGa>(ApplyStatus, ReactionTiming.POST);
    }

    private void OnDisable()
    {
        ActionSystem.UnsubscribeReaction<StartTurnGa>(ApplyStatus, ReactionTiming.POST);
    }

    private void ApplyStatus(StartTurnGa turn)
    {
        if (turn.starting == Combat.CombatLoop.TurnType.Player)
            return;

        for (int i = 0; i < targetAmount; i++)
        {
            CardMovement card = TargetingSystem.instance.RetrieveRandomCard(Combat.Spells.TargetType.Ally);

            ApplyStatusGa applyStatusGa = new ApplyStatusGa(status, stackAmount, cardController, card.cardController);
            ActionSystem.instance.AddReaction(applyStatusGa);

            if (multipleTarget == false) 
                break;
        }
    }
}
