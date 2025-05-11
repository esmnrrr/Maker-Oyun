using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;
    public float crouchHeight = 1f;
    public float normalHeight = 2f;

    [Header("Attack Settings")]
    public float attackCooldown = 1f;

    private CharacterController controller;
    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isSprinting;
    private bool isCrouching;
    private bool canAttack = true;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private int damage = 20;
    [SerializeField] private LayerMask enemyLayers;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputActions = new PlayerInputActions();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Jump.performed += ctx => Jump();
        inputActions.Player.Crouch.performed += ctx => ToggleCrouch();
        inputActions.Player.Sprint.performed += ctx => isSprinting = true;
        inputActions.Player.Sprint.canceled += ctx => isSprinting = false;
        inputActions.Player.Attack.performed += ctx => Attack();
    }

    void OnEnable() => inputActions.Enable();
    void OnDisable() => inputActions.Disable();
    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        float currentSpeed = isSprinting ? runSpeed : walkSpeed;

        if (move != Vector3.zero)
        {
            // Yönü direkt değiştir (kamera değil, dünya yönüne göre)
            transform.rotation = Quaternion.LookRotation(move);
            controller.Move(move.normalized * currentSpeed * Time.deltaTime);
        }

        // Yerçekimi
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    /*   void Update()
       {
           isGrounded = controller.isGrounded;
           if (isGrounded && velocity.y < 0)
               velocity.y = -2f;

           Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
           float currentSpeed = isSprinting ? runSpeed : walkSpeed;

           controller.Move(move * currentSpeed * Time.deltaTime);

           // Yöne dönme işlemi
           if (move != Vector3.zero)
           {
               Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
               transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 10f);
           }

           // Yerçekimi
           velocity.y += gravity * Time.deltaTime;
           controller.Move(velocity * Time.deltaTime);
     */
//}


void Jump()
    {
        if (isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    void ToggleCrouch()
    {
        isCrouching = !isCrouching;
        controller.height = isCrouching ? crouchHeight : normalHeight;
    }

    void Attack()
    {
        if (!canAttack) return;
        Debug.Log("Player attacked!");
        canAttack = false;

        // Düşman tespiti ve hasar verme
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            EnemyHealth health = enemy.GetComponent<EnemyHealth>();

            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }

        Invoke(nameof(ResetAttack), attackCooldown);
        
    }


    void ResetAttack() => canAttack = true;
}
