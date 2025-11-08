using ActionReaction;
using System;
using ActionReaction.Game_Actions;
using Run_Loop;
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
        public int currentHealth { get; private set; }

        public bool IsDead => currentHealth <= 0;

        private void Start()
        {
            cardController = GetComponent<CardController>();
        }

        public void Setup(DeckCard deckCard)
        {
            currentHealth = deckCard.currentHealth;
            OnUpdateHP.Invoke(currentHealth);
        }

        public void TakeDamage(int damage, CardController attacker)
        {
            if (IsDead)
                return;

            damage = Mathf.Max(0, damage);
            
            currentHealth -= damage;
            DamageNumberFactory.instance.DisplayDamageNumber(cardController.screenPosition, damage);
            OnUpdateHP.Invoke(currentHealth);

            if (IsDead)
            {
                DeathGA death = new DeathGA(attacker, cardController);
                ActionSystem.instance.AddReaction(death);
            }
        }

        public void Heal(int heal)
        {
            if (IsDead)
                return;

            currentHealth += heal;

            if (currentHealth > cardController.cardData.hpMax)
                currentHealth = cardController.cardData.hpMax;

            DamageNumberFactory.instance.DisplayHealNumber(cardController.screenPosition, heal);
            OnUpdateHP.Invoke(currentHealth);
        }

        public void Dies()
        {
            OnDeath.Invoke();
        }
    }
}
