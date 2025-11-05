using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using EnemyAttack;
using Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellStrike : SpellController
{
    [SerializeField] private int attackBuff;

    protected override void SubscribeReactions()
    {
        base.SubscribeReactions();
        ActionSystem.SubscribeReaction<DeathGA>(AngerReaction, ReactionTiming.POST);
    }

    protected override void UnsubscribeReactions()
    {
        base.UnsubscribeReactions();
        ActionSystem.UnsubscribeReaction<DeathGA>(AngerReaction, ReactionTiming.POST);
    }

    protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
    {
        yield return base.CastSpellOnTarget(targets);

        foreach (CardMovement target in targets)
        {
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            Debug.Log($"Target : {target.cardController.cardData.cardName} / {spellData.targetType}");

            DealDamageGA damageGa = new DealDamageGA(ComputeCurrentDamage(spellData.damage), cardController, target.cardController);
            ActionSystem.instance.Perform(damageGa);
        }
    }

    private void AngerReaction(DeathGA gA)
    {
        ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.PermanentBonusDamage, attackBuff, cardController, cardController);
        ActionSystem.instance.AddReaction(applyStatusGa);
        Debug.Log("TOUPANGRY");
    }
}
