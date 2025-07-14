// State.cs (Input System + SphereCast ile güncellenmiş versiyon)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class State
{
    public enum STATE { GROUNDED, AIRBORNE, IDLE, WALK, RUN, CROUCH, ATTACK, JUMP, AIRATTACK };
    public enum EVENT { ENTER, UPDATE, EXIT };

    public STATE name;
    protected EVENT stage;
    protected GameObject player;
    protected CharacterController controller;
    protected Animator anim;
    protected State nextState;
    protected State superState;
    protected State subState;
    public Transform cam;

    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float currentSpeed;
    public float gravity = -9.81f;
    protected float ySpeed = 0f;

    protected static PlayerInputActions inputActions;

    public static void SetInputActions(PlayerInputActions actions)
    {
        inputActions = actions;
    }

    public State(GameObject _player, Animator _anim, CharacterController _controller, Transform _cam)
    {
        player = _player;
        anim = _anim;
        controller = _controller;
        cam = _cam;
        stage = EVENT.ENTER;
    }

    protected Vector2 MovementInput => inputActions != null ? inputActions.Player.Move.ReadValue<Vector2>() : Vector2.zero;

    protected bool MovementKeyPressed => MovementInput != Vector2.zero;

    protected void HandleSpeed()
    {
        if (!MovementKeyPressed)
        {
            currentSpeed = 0f;
        }
        else if (inputActions.Player.Sprint.IsPressed())
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }
    }

    public void MovePlayer(float? customYSpeed = null)
    {
        float horizontal = MovementInput.x;
        float vertical = MovementInput.y;

        Vector3 moveDirection = Vector3.zero;

        if (vertical != 0)
        {
            Vector3 forward = cam.transform.forward;
            forward.y = 0;
            forward.Normalize();
            moveDirection = forward * vertical;
        }

        if (horizontal != 0)
        {
            float rotationSpeed = 200f;
            player.transform.Rotate(0, horizontal * rotationSpeed * Time.deltaTime, 0);
        }

        float usedYSpeed = customYSpeed ?? ySpeed;

        if (!IsGrounded())
        {
            ySpeed += gravity * Time.deltaTime;
            usedYSpeed = customYSpeed ?? ySpeed;
        }
        else
        {
            if (ySpeed < 0)
                ySpeed = -2f;
            usedYSpeed = customYSpeed ?? ySpeed;
        }

        Vector3 finalVelocity = new Vector3(moveDirection.x * currentSpeed, usedYSpeed, moveDirection.z * currentSpeed);
        controller.Move(finalVelocity * Time.deltaTime);

        if (moveDirection.magnitude >= 0.1f && vertical > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.deltaTime * 10f);

            Vector3 targetCamRotation = new Vector3(0, player.transform.eulerAngles.y, 0);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, Quaternion.Euler(targetCamRotation), Time.deltaTime * 5f);
        }
    }

    protected bool IsGrounded()
    {
        // Başlangıç noktası: karakterin tam altı, biraz yukarıdan
        Vector3 rayOrigin = player.transform.position + Vector3.up * 0.1f;

        // Yarıçap: çok küçük olursa kenardan kaçırır, çok büyük olursa erken temas eder
        float sphereRadius = controller.radius * 0.5f;

        // Uzunluk: yere ulaşması için yeterli olmalı
        float rayLength = 0.6f;

        // Kendi layer'ını hariç tut
        int layerMask = ~(1 << player.layer);

        // Debug çizgisi
        Debug.DrawRay(rayOrigin, Vector3.down * rayLength, Color.red);

        // Temas kontrolü
        return Physics.SphereCast(rayOrigin, sphereRadius, Vector3.down, out RaycastHit hit, rayLength, layerMask);
    }





    public void SetSuperState(State super) => superState = super;

    public void SetSubState(State sub)
    {
        subState = sub;
        sub.SetSuperState(this);
        sub.Enter();
    }

    public void SwitchSubState(State newSubState)
    {
        subState?.Exit();
        SetSubState(newSubState);
    }

    public virtual void Enter()
    {
        stage = EVENT.UPDATE;
        subState?.Enter();
    }

    public virtual void Update()
    {
        HandleSpeed();
        subState?.Update();
    }

    public virtual void Exit()
    {
        subState?.Exit();
        stage = EVENT.EXIT;
    }

    public virtual State Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }

    public void SetNextState(State next) => nextState = next;
}