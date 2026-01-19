using System.Collections;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace EnemyAttack.Behaviours
{
    public class LifeStealBehaviour : DealDamageBehaviour
    {
        public override IEnumerator ExecuteBehavior()
        {
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            CardController target = ComputeTarget();

            if (target == null)
                yield break;

            DealDamageGA damageGa = new DealDamageGA(ComputeCurrentDamage(damage, target), enemyCardController.cardController, target);
            ActionSystem.instance.Perform(damageGa, () => OnPerformFinished(damageGa));
        }

        private void OnPerformFinished(DealDamageGA damageGA)
        {
            HealGa healGa = new HealGa(damageGA.amount, enemyCardController.cardController, enemyCardController.cardController);
            ActionSystem.instance.Perform(healGa);
        }
    }
}
