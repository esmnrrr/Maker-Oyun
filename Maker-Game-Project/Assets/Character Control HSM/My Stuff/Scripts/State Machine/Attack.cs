// Attack.cs – FSM'e uygun saldırı durumu

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : State
{
    private float attackCooldown = 1.5f; // saldırılar arası süre
    private float lastAttackTime = -Mathf.Infinity;
    private float attackRange = 1.5f; // saldırı menzili
    private int attackDamage = 10;

    private Transform attackPoint;
    private LayerMask enemyLayer;

    public Attack(GameObject _player, Animator _anim, CharacterController _controller, Transform _cam)
        : base(_player, _anim, _controller, _cam)
    {
        name = STATE.ATTACK;

        // AttackPoint'i FSM üzerinden erişimle ayarlıyoruz
        attackPoint = player.transform.Find("AttackPoint");
        enemyLayer = LayerMask.GetMask("Enemy");
    }

    public override void Enter()
    {
        base.Enter();
        PerformAttack();
        // anim.SetTrigger("Attack");
    }


    public override void Update()
    {
        base.Update();

        // Saldırıdan sonra yürüyüşe dön
        SetNextState(new Grounded(player, anim, controller, cam));
        stage = EVENT.EXIT;
    }

    private void PerformAttack()
    {
        if (attackPoint == null)
        {
            Debug.LogWarning("AttackPoint bulunamadı!");
            return;
        }

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>()?.TakeDamage(attackDamage);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    // Editor'de menzili görselleştirme
#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
#endif
}
