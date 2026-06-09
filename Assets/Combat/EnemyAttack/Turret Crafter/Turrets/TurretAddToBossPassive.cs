using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;
using Combat.Spells.Targeting;
using UnityEngine;

public class TurretAddToBossPassive : PassiveController
{
    [SerializeField] private CardData bossCardData;
    [SerializeField] private PassiveData turretPassiveData;

    public override void Setup(CardController controller, PassiveData data)
    {
        base.Setup(controller, data);
    }

    private void OnEnable()
    {
        ActionSystem.SubscribeReaction<DeathGA>(RemoveAdditionalPassiveFromBoss, ReactionTiming.PRE);
        ActionSystem.SubscribeReaction<SpawnCardGA>(CreateAdditionalPassiveForBoss, ReactionTiming.POST);
    }

    private void OnDisable()
    {
        ActionSystem.UnsubscribeReaction<DeathGA>(RemoveAdditionalPassiveFromBoss, ReactionTiming.PRE);
        ActionSystem.UnsubscribeReaction<SpawnCardGA>(CreateAdditionalPassiveForBoss, ReactionTiming.POST);
    }

    private void RemoveAdditionalPassiveFromBoss(DeathGA deathGa)
    {
        if (deathGa.target != cardController)
            return;

        CardMovement bossCard = TargetingSystem.instance.RetrieveCard(bossCardData, Combat.Spells.TargetType.Enemy);

        RemovePassiveGa passiveGa = new RemovePassiveGa(cardController, bossCard.cardController, turretPassiveData);
        ActionSystem.instance.AddReaction(passiveGa);
    }

    private void CreateAdditionalPassiveForBoss(SpawnCardGA spawnCardGA)
    {
        if (spawnCardGA.spawnedCard != cardController)
            return;

        CardMovement bossCard = TargetingSystem.instance.RetrieveCard(bossCardData, Combat.Spells.TargetType.Enemy);

        ApplyPassiveGa passiveGa = new ApplyPassiveGa(cardController, bossCard.cardController, turretPassiveData);
        ActionSystem.instance.AddReaction(passiveGa);
    }
}
