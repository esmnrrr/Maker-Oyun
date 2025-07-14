using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : State
{
    private PlayerFSMController fsm;

    public Grounded(GameObject _player, Animator _anim, CharacterController _controller, Transform _cam)
        : base(_player, _anim, _controller, _cam)
    {
        name = STATE.GROUNDED;
    }

    public override void Enter()
    {
        base.Enter();

        fsm = player.GetComponent<PlayerFSMController>();

        if (!MovementKeyPressed)
            SetSubState(new Idle(player, anim, controller, cam));
        else if (inputActions.Player.Sprint.IsPressed())
            SetSubState(new Run(player, anim, controller, cam));
        else
            SetSubState(new Walk(player, anim, controller, cam));
    }

    public override void Update()
    {
        //Debug.Log("Grounded update");//
        base.Update();

        // Eğer artık yerde değilsek -> Jump state'ine geç
        if (!IsGrounded())
        {
            SetNextState(new Jump(player, anim, controller, cam));
            stage = EVENT.EXIT;
            return;
        }

        // Eğer saldırı input'u gelirse ve cooldown dolduysa -> Attack state'ine geç
        if (inputActions.Player.Attack.triggered &&
            Time.time >= fsm.lastAttackTime + fsm.attackCooldown)

        {
            fsm.lastAttackTime = Time.time;
            SetNextState(new Attack(player, anim, controller, cam));
            stage = EVENT.EXIT;
            return;
        }

        if (!MovementKeyPressed)
            SwitchSubState(new Idle(player, anim, controller, cam));
        else if (inputActions.Player.Sprint.IsPressed())
            SwitchSubState(new Run(player, anim, controller, cam));
        else
            SwitchSubState(new Walk(player, anim, controller, cam));
    }
}
