using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat;
using Combat.EnemyAttack;
using Combat.Passives;
using System.Collections;
using UnityEngine;

public class ThornPassive : PassiveController
{
    [SerializeField] private int damageAmount;

    private void OnEnable()
    {
        ActionSystem.SubscribeReaction<DealDamageGA>(TryReflectDamage, ReactionTiming.POST);
    }

    private void OnDisable()
    {
        ActionSystem.UnsubscribeReaction<DealDamageGA>(TryReflectDamage, ReactionTiming.POST);
    }

    private void TryReflectDamage(DealDamageGA gA)
    {
        var damagePackage = gA.GetDamagePackageForTarget(cardController);

        if (damagePackage == null)
            return;

        if (!damagePackage.isDamageNegated)
        {
            DealDamageGA dealDamage = new DealDamageGA(damageAmount, cardController, gA.attacker);
            ActionSystem.instance.AddReaction(dealDamage);
        }
    }
}
