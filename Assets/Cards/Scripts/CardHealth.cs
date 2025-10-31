using UnityEngine;
using UnityEngine.Events;

namespace Cards.Scripts
{
    public class CardHealth : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<int> OnUpdateHP = new UnityEvent<int>();
        [HideInInspector] public UnityEvent OnDeath = new UnityEvent();

        private int currentHealth;

        public bool IsDead => currentHealth <= 0;

        public void Setup(CardData data)
        {
            currentHealth = data.hpMax;
            OnUpdateHP.Invoke(currentHealth);
        }

        public void TakeDamage(int damage)
        {
            if (IsDead)
                return;
        
            currentHealth -= damage;
            OnUpdateHP.Invoke(currentHealth);

            if (IsDead)
                Dies();
        }

        private void Dies()
        {
            OnDeath.Invoke();
        }
    }
}
