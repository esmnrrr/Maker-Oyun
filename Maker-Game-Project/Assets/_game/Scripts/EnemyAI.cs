using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2f;
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;
    public int damage = 20;

    private Rigidbody rb;
    private float nextAttackTime = 0f;
    private EnemyHealth health;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Dönmeleri engelle
        health = GetComponent<EnemyHealth>();
    }

    void FixedUpdate()
    {
        if (player == null || health == null || health.isDefeated) return;

        float distance = Vector3.Distance(transform.position, player.position);

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f; // Yalnızca yatay düzlemde yönlenme

        // Oyuncuya dön
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);

        if (distance > attackRange)
        {
            // Rigidbody ile hareket et
            Vector3 targetPosition = rb.position + direction * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPosition);
        }
        else if (Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void Attack()
    {
        Debug.Log("Enemy attacks player!");

        Health playerHealth = player.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }
}
