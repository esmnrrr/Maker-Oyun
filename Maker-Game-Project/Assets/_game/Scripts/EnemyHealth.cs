using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;

    [Header("State")]
    public bool isDefeated = false;
    public bool isDead = false;
    private bool hasEverBeenDefeated = false;

    private float defeatedProtectionTime = 2f; // Teslim olduktan sonra koruma süresi
    private float defeatedStartTime;
    private bool isInDefeatedProtection = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        // Eğer ölmüşse hasar alma
        /* if (isDead) return; */

        // Eğer teslim olmuşsa ve koruma süresi aktifse hasar alma
        if (isDefeated && isInDefeatedProtection)
        {
            Debug.Log($"{gameObject.name} teslim oldu. Geçici koruma aktif, hasar almadı.");
            return;
        }

        // Eğer teslim olmuş ama koruma süresi bitti ise: tekrar savaş durumu
        if (isDefeated && !isInDefeatedProtection)
        {
            isDefeated = false;
            Debug.Log($"{gameObject.name} teslim durumundan çıkarıldı. Savaşmaya devam ediyor!");
        }

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log($"{gameObject.name} took {amount} damage. Current health: {currentHealth}");

        // Teslimiyet (sadece ilk kez teslim olacaksa)
        if (!isDefeated && !hasEverBeenDefeated && currentHealth <= 50 && currentHealth > 0)
        {
            isDefeated = true;
            hasEverBeenDefeated = true;
            isInDefeatedProtection = true;
            defeatedStartTime = Time.time;
            Debug.Log($"{gameObject.name} teslim oldu. 2 saniyelik koruma başladı.");
            return;
        }

        // Ölüm
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Update()
    {
        // Teslim koruma süresi izleniyor
        if (isDefeated && isInDefeatedProtection && Time.time >= defeatedStartTime + defeatedProtectionTime)
        {
            isInDefeatedProtection = false;
            Debug.Log($"{gameObject.name} teslimiyet koruması sona erdi. Artık saldırılırsa savaşır.");
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("[Death] Enemy öldü!");

        // AI’yi durdur
        EnemyAI enemyAI = GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.enabled = false;
            Debug.Log("[Die] EnemyAI devre dışı bırakıldı.");
        }

        // Rigidbody varsa hareketi durdur
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
        }

        // Collider’ı pasifleştir
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        /* (İsteğe bağlı) animasyon varsa tetikle
        if (animator != null)
        {
            animator.SetTrigger("Die"); // animasyon varsa
        }
        */
        // İleride burada: “Loot bırak”, “XP ver”, “Objeyi 3 sn sonra yok et” gibi şeyler de olabilir
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
