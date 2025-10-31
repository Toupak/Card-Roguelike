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
