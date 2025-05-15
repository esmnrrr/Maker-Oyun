using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Grounded : State
{
    public Grounded(GameObject _player, Animator _anim, CharacterController _controller, Transform _cam)
        : base(_player, _anim, _controller,_cam)
    {
        name = STATE.GROUNDED;
    }

    public override void Enter()
    {
        base.Enter();
        if (!MovementKeyPressed)
            SetSubState(new Idle(player, anim, controller,cam));
        else if (Input.GetKey(KeyCode.LeftShift))
            SetSubState(new Run(player, anim, controller,cam));
        else
            SetSubState(new Walk(player, anim, controller,cam));
    }

    public override void Update()
    {
        base.Update();
        
        if (!MovementKeyPressed)
            SwitchSubState(new Idle(player, anim, controller, cam));
        else if (Input.GetKey(KeyCode.LeftShift))
            SwitchSubState(new Run(player, anim, controller, cam));
        else
            SwitchSubState(new Walk(player, anim, controller, cam));

        // (optional) handle jump or leave-ground detection here
    }
}
