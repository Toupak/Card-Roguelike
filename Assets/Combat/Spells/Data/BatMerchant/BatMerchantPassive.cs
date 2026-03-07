using System;
using ActionReaction;
using ActionReaction.Game_Actions;
using Combat.Passives;
using UnityEngine;

public class BatMerchantPassive : PassiveController
{
    private void OnEnable()
    {
        ActionSystem.SubscribeReaction<DeathGA>(DeathReaction, ReactionTiming.POST);
    }

    private void OnDisable()
    {
        ActionSystem.UnsubscribeReaction<DeathGA>(DeathReaction, ReactionTiming.POST);
    }

    private void DeathReaction(DeathGA deathGA)
    {
        if (deathGA.killer == cardController)
        {
            if (cardController.singleButton.spellController != null)
                cardController.singleButton.spellController.SetShinyState(true);
            if (cardController.leftButton.spellController != null)
                cardController.leftButton.spellController.SetShinyState(true);
            if (cardController.rightButton.spellController != null)
                cardController.rightButton.spellController.SetShinyState(true);
        }
    }
}
