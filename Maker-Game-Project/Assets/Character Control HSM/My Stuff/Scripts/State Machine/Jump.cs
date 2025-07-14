using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : State
{
    public float jumpForce = 10f;

    public Jump(GameObject _player, Animator _anim, CharacterController _controller, Transform _cam)
        : base(_player, _anim, _controller, _cam)
    {
        name = STATE.JUMP;
    }

    public override void Enter()
    {
        base.Enter();

        if (IsGrounded())
        {
            ySpeed = jumpForce;  // yalnızca zıplama tuşuyla tetiklenmişse
            // anim.Play("Jump");
        }
        else
        {
            ySpeed = -2f; // Boşluğa yürümeyse: yavaş düşüş başlat
        }
    }

    public override void Update()
    {
        base.Update();

        // Yerçekimi
        ySpeed += gravity * Time.deltaTime;

        // Hareket
        MovePlayer(ySpeed);

        // Yere indik mi kontrolü
        if (IsGrounded() && ySpeed < 0)
        {
            SetNextState(new Grounded(player, anim, controller, cam));
            stage = EVENT.EXIT;
        }
    }
}
