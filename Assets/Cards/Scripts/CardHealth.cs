using System;
using UI.Damage_Numbers;
using UnityEngine;
using UnityEngine.Events;

namespace Cards.Scripts
{
    public class CardHealth : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<int> OnUpdateHP = new UnityEvent<int>();
        [HideInInspector] public UnityEvent OnDeath = new UnityEvent();
        
        private CardController cardController;
        private int currentHealth;

        public bool IsDead => currentHealth <= 0;

        private void Start()
        {
            cardController = GetComponent<CardController>();
        }

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
            DamageNumberFactory.instance.DisplayDamageNumber(cardController.screenPosition, damage);
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
