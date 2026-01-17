using System.Collections;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace EnemyAttack.Monk_Fight.Monk
{
    public class AngerBehaviour : BaseEnemyBehaviour
    {
        [SerializeField] private int damageBuff;

        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<DeathGA>(AngerReaction, ReactionTiming.POST);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<DeathGA>(AngerReaction, ReactionTiming.POST);
        }

        private void AngerReaction(DeathGA gA)
        {
            if (gA.isEnemy)
            {
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.PermanentBonusDamage, damageBuff, enemyCardController.cardController, enemyCardController.cardController);
                ActionSystem.instance.AddReaction(applyStatusGa);
            }
        }

        public override IEnumerator ExecuteBehavior()
        {
            yield break;
        }
    }
}
