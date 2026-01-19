using System.Collections;
using ActionReaction;
using ActionReaction.Game_Actions;
using Combat.EnemyAttack.Behaviours;
using UnityEngine;

namespace Combat.EnemyAttack.Angry_Plant
{
    public class LifeDrain : DealDamageBehaviour
    {
        [SerializeField] private int healAmount;

        public override IEnumerator ExecuteBehavior()
        {
            yield return base.ExecuteBehavior();

            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            HealGa healGa = new HealGa(healAmount, enemyCardController.cardController, enemyCardController.cardController);
            ActionSystem.instance.Perform(healGa);
        }
    }
}
