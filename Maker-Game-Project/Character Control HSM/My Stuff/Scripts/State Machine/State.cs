using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public enum STATE
    {
        GROUNDED, AIRBORNE, IDLE, WALK, RUN, CROUCH, ATTACK, JUMP, AIRATTACK
    };

    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    };

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
    public float gravity = -200f; 
    private Vector3 velocity;       
    private float ySpeed = 0f;     

    public State(GameObject _player, Animator _anim, CharacterController _controller, Transform _cam)
    {
        player = _player;
        anim = _anim;
        controller = _controller;
        stage = EVENT.ENTER;
        cam = _cam;
    }

    protected Vector2 MovementInput => new Vector2(
        Input.GetAxisRaw("Horizontal"),
        Input.GetAxisRaw("Vertical")
    );

    protected bool MovementKeyPressed =>
        MovementInput.x != 0 || MovementInput.y != 0;

    protected void HandleSpeed()
    {
        if (!MovementKeyPressed)
        {
            currentSpeed = 0f;  
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }
    }

    public void MovePlayer()
    {
        
        float horizontal = MovementInput.x;
        float vertical = MovementInput.y;

        
        Vector3 forward = cam.transform.forward;
        Vector3 right = cam.transform.right;

        
        forward.y = 0;
        right.y = 0;

        
        forward.Normalize();
        right.Normalize();

        
        Vector3 moveDirection = (forward * vertical + right * horizontal).normalized;
        Debug.Log("Move Direction: " + moveDirection);
        

        //controller.Move(moveDirection * currentSpeed * Time.deltaTime);
        //Debug.Log("Current Speed: " + currentSpeed);

        /*if (!controller.isGrounded)
        {
            ySpeed += gravity * Time.deltaTime;  
        }
        else
        {
            
            if (ySpeed < 0)
                ySpeed = -2f;  
        }*/

        
        Vector3 finalVelocity = new Vector3(moveDirection.x * currentSpeed, ySpeed, moveDirection.z * currentSpeed);

        
        

        if (moveDirection.magnitude >= 0.1f)
        {

            //controller.Move(moveDirection * currentSpeed * Time.deltaTime);
            controller.Move(finalVelocity * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.deltaTime * 10f);

            
            Vector3 targetCamRotation = new Vector3(0, player.transform.eulerAngles.y, 0); // Keep the camera y-rotation matching player
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, Quaternion.Euler(targetCamRotation), Time.deltaTime * 5f); // Smooth camera rotation
        }
        




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




