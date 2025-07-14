using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : State
{
    public Run(GameObject _player, Animator _anim, CharacterController _controller, Transform _cam)
        : base(_player, _anim, _controller, _cam)
    {
        name = STATE.RUN;
    }

    public override void Enter()
    {
        base.Enter();
        //anim.Play("isRunning");
    }

    public override void Update()
    {
        base.Update();
        MovePlayer(); // inputActions üzerinden gelen input ile otomatik çalışır
    }
}
