using System.Collections;

namespace Combat.EnemyAttack
{
    public interface IEnemyBehaviour
    {
        public void Setup(EnemyCardController enemyCardController);
        public bool CanBeSelected();
        public IEnumerator ExecuteBehavior();
    }
}
