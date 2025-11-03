using ActionReaction;
using ActionReaction.Game_Actions;
using BoomLib.Tools;
using Cards.Scripts;
using Spells;
using System;
using UnityEngine;

public class SlipperyTrap : SpellController
{
    private void OnEnable()
    {
        ActionSystem.SubscribeReaction<DealDamageGA>(SlipperyTrapReaction, ReactionTiming.POST);
    }

    private void OnDisable()
    {
        ActionSystem.UnsubscribeReaction<DealDamageGA>(SlipperyTrapReaction, ReactionTiming.POST);
    }

    private void SlipperyTrapReaction(DealDamageGA gA)
    {
        if (gA.target == cardController)
        {
            bool random = Tools.RandomBool();

            if (random)
            {
                ApplyStatusGa stun = new ApplyStatusGa(StatusType.Stun, 2, cardController, gA.attacker);
                ActionSystem.instance.AddReaction(stun);
            }
        }
    }

}
