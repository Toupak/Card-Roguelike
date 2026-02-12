using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;
using System.Collections.Generic;
using UnityEngine;

public class AlluringAroma : PassiveController
{
    [SerializeField] private int hpBeforePhase2;

    [SerializeField] protected CardData bigSpiderCardData;
    [SerializeField] protected CardData spiderlings;

    [SerializeField] protected CardData cocon;   

    private void OnEnable()
    {
        ActionSystem.SubscribeReaction<DealDamageGA>(TryTriggerPhase2, ReactionTiming.POST);
    }

    private void OnDisable()
    {
        ActionSystem.UnsubscribeReaction<DealDamageGA>(TryTriggerPhase2, ReactionTiming.POST);
    }

    private void TryTriggerPhase2(DealDamageGA damage)
    {
        if (damage.GetPackageFromTarget(cardController) != null)
        {
            if (cardController.cardHealth.currentHealth <= hpBeforePhase2)
                SpawnPhase2();
        }
    }

    private void SpawnPhase2()
    {
        //Spawn 1 spiderling / 1 spider twice
        for (int i = 0; i < 2; i++)
        {
            SpawnCardGA spawnSpiderling = new SpawnCardGA(spiderlings, cardController);
            ActionSystem.instance.AddReaction(spawnSpiderling);

            SpawnCardGA spawnSpider = new SpawnCardGA(bigSpiderCardData, cardController);
            ActionSystem.instance.AddReaction(spawnSpider);
        }

        SpawnCardGA spawnCocon = new SpawnCardGA(cocon, cardController);
        ActionSystem.instance.AddReaction(spawnCocon);

        cardController.KillCard();
    }
}
