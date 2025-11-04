using ActionReaction;
using Cards.Scripts;
using UnityEngine;

public class DeathGA : GameAction
{
    public CardController target;
    public bool isEnemy;

    public DeathGA(CardController deathTarget)
    {
        target = deathTarget;
        isEnemy = target.cardMovement.IsEnemyCard;
    }
}
