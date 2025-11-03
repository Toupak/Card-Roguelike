using Cards.Scripts;
using Status;
using System.Collections;
using UnityEngine;

namespace EnemyAttack
{
    public class BaseEnemyBehaviour : MonoBehaviour, IEnemyBehaviour
    {
        public string behaviourName;
        [TextArea] public string description;
        public int weight;
        public Sprite intentionIcon;

        protected EnemyCardController enemyCardController;

        public virtual void Setup(EnemyCardController controller)
        {
            enemyCardController = controller;
        }

        protected virtual int ComputeCurrentDamage(int spellDamage)
        {
            int bonus = 0;

            if (StatusSystem.instance.IsCardAfflictedByStatus(enemyCardController.cardController, StatusType.BonusDamage))
                bonus += enemyCardController.cardController.cardStatus.currentStacks[StatusType.BonusDamage];

            return spellDamage + bonus;
        }

        public virtual bool CanBeSelected()
        {
            return true;
        }

        public virtual IEnumerator Execute()
        {
            yield break;
        }
    }
}
