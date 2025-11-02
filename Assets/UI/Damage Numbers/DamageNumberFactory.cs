using UnityEngine;

namespace UI.Damage_Numbers
{
    public class DamageNumberFactory : MonoBehaviour
    {
        [SerializeField] private DamageNumberDisplay damageNumberDisplayPrefab;
        
        public static DamageNumberFactory instance;

        private void Start()
        {
            instance = this;
        }

        public void DisplayDamageNumber(Vector2 position, int damage)
        {
            Instantiate(damageNumberDisplayPrefab, transform).Setup(position, damage);
        }
    }
}
