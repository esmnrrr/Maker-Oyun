using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
{
    public Idle(GameObject _player, Animator _anim, CharacterController _controller, Transform _cam)
        : base(_player, _anim, _controller, _cam)
    {
        name = STATE.IDLE;
    }

    public override void Enter()
    {
        //anim.Play("isIdle");
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        // İleriye hareket veya başka bir input gelirse Idle'dan çıkılabilir
        // Şimdilik sadece sabit durur
        controller.Move(Vector3.zero);
    }
}
