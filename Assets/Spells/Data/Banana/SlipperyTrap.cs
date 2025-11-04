using ActionReaction;
using ActionReaction.Game_Actions;
using BoomLib.Tools;
using Cards.Scripts;
using Spells;
using System;
using UnityEngine;

public class SlipperyTrap : SpellController
{
    protected override void SubscribeReactions()
    {
        base.SubscribeReactions();
        ActionSystem.SubscribeReaction<DealDamageGA>(SlipperyTrapReaction, ReactionTiming.POST);
    }

    protected override void UnsubscribeReactions()
    {
        base.UnsubscribeReactions();
        ActionSystem.UnsubscribeReaction<DealDamageGA>(SlipperyTrapReaction, ReactionTiming.POST);
    }

    private void SlipperyTrapReaction(DealDamageGA dealDamageGa)
    {
        if (dealDamageGa.target == cardController)
        {
            bool random = Tools.RandomBool();

            if (random)
            {
                dealDamageGa.NegateDamage();
                ApplyStatusGa stun = new ApplyStatusGa(StatusType.Stun, 2, cardController, dealDamageGa.attacker);
                ActionSystem.instance.AddReaction(stun);
            }
        }
    }
}
