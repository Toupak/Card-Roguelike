using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Burnout : SpellController

{
    protected override IEnumerator CastSpellOnTarget(SpellData spellData, List<CardMovement> targets)
    {
        yield return base.CastSpellOnTarget(spellData, targets);

        int burnoutDamage = (cardController.cardData.hpMax - cardController.cardHealth.currentHealth) / 2;

        foreach (CardMovement target in targets)
        {
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            Debug.Log($"Target : {target.cardController.cardData.cardName} / {spellData.targetType}");
            DealDamageGA damageGa = new DealDamageGA(ComputeCurrentDamage(spellData.damage + burnoutDamage), cardController, target.cardController);
            ActionSystem.instance.Perform(damageGa);
        }

        yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

        DealDamageGA selfDestroyGa = new DealDamageGA(9000, cardController, cardController);
        ActionSystem.instance.Perform(selfDestroyGa);
    }
}
