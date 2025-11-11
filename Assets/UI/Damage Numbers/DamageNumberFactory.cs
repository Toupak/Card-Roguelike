using UnityEngine;

namespace UI.Damage_Numbers
{
    public class DamageNumberFactory : MonoBehaviour
    {
        [SerializeField] private DamageNumberDisplay damageNumberDisplayPrefab;
        [SerializeField] private DamageNumberDisplay healNumberDisplayPrefab;
        [SerializeField] private DamageNumberDisplay MissDisplayPrefab;
        
        public static DamageNumberFactory instance;

        private void Start()
        {
            instance = this;
        }

        public void DisplayDamageNumber(Vector2 position, int damage)
        {
            Instantiate(damageNumberDisplayPrefab, transform).Setup(position, damage, false);
        }
        
        public void DisplayHealNumber(Vector2 position, int damage)
        {
            Instantiate(healNumberDisplayPrefab, transform).Setup(position, damage, true);
        }

        public void DisplayQuickMessage(Vector2 position, string text)
        {
            Instantiate(MissDisplayPrefab, transform).Setup(position, text);
        }
    }
}
