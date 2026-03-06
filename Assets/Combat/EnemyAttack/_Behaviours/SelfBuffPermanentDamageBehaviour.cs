using System.Collections;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.EnemyAttack.Behaviours
{
    public class SelfBuffPermanentDamageBehaviour : BaseEnemyBehaviour
    {
        [SerializeField] private int damageStacks;

        public override IEnumerator ExecuteBehavior()
        {
            Debug.Log("SelfBuffPermanentDamage");
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.PermanentBonusDamage, damageStacks, enemyCardController.cardController, enemyCardController.cardController);
            ActionSystem.instance.Perform(applyStatusGa);
        }
    }
}
