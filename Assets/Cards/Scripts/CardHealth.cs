using Data;
using UnityEngine;
using UnityEngine.Events;

public class CardHealth : MonoBehaviour
{
    [HideInInspector] public UnityEvent<int> OnUpdateHP = new UnityEvent<int>();
    [HideInInspector] public UnityEvent OnDeath = new UnityEvent();

    private int currentHealth;

    public void Setup(CardData data)
    {
        currentHealth = data.hpMax;
        OnUpdateHP.Invoke(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        OnUpdateHP.Invoke(currentHealth);

        if (currentHealth <= 0)
            Dies();
    }

    private void Dies()
    {
        OnDeath.Invoke();
    }
}
