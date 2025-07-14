using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Oyuncu Can Ayarları")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Animasyon (isteğe bağlı)")]
    public Animator animator;

    private void Start()
    {
        currentHealth = maxHealth;

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;

        // 🔽 LOG: Hasar bilgisi
        Debug.Log($"[Damage] Oyuncu {damage} hasar aldı. Kalan can: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("[Death] Oyuncu öldü!");

        if (animator != null)
            animator.SetTrigger("Die");

        // Karakter kontrolünü durdur
        PlayerFSMController fsm = GetComponent<PlayerFSMController>();
        if (fsm != null)
        {
            enabled = false; // Sağlık scriptini durdur
            fsm.enabled = false; // FSM durur → hareket vs olmaz
        }

        // Geçici olarak sahneyi resetle (isteğe bağlı)
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Gelecekte buraya UI gelecek: Game Over ekranı vs.
    }

}
