using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : State
{
    public Walk(GameObject _player, Animator _anim, CharacterController _controller, Transform _cam)
        : base(_player, _anim, _controller, _cam)
    {
        name = STATE.WALK;
    }

    public override void Enter()
    {
        base.Enter();
        //anim.Play("isWalking");
    }

    public override void Update()
    {
        base.Update();
        MovePlayer(); // inputActions içindeki Move input'una göre çalışır
    }
}
