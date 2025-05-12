using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : State
{
    
    
    public Run(GameObject _player, Animator _anim, CharacterController _controller,Transform _cam)
        : base(_player, _anim, _controller,_cam)
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
        /*Vector3 move = new Vector3(MovementInput.x, 0, MovementInput.y).normalized;
        controller.Move(move * walkSpeed * Time.deltaTime);
        if (move != Vector3.zero)
            player.transform.forward = move;*/
        MovePlayer();
    }
}
