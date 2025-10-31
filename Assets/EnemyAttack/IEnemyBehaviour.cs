using System.Collections;

namespace EnemyAttack
{
    public interface IEnemyBehaviour
    {
        public void Setup(EnemyCardController enemyCardController);
        public bool CanBeSelected();
        public IEnumerator Execute();
    }
}
