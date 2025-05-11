using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;

    [SerializeField] private int currentHealth;

    [Header("State")]
    public bool isDefeated = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (isDefeated) return;

        currentHealth -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
        else if (currentHealth <= 50)
        {
            isDefeated = true;
            Debug.Log($"{gameObject.name} is defeated and stops fighting.");
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        // Destroy(gameObject);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
