using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // 🔹 Input System import

[RequireComponent(typeof(CharacterController))]
public class PlayerFSMController : MonoBehaviour
{
    public Animator anim;
    private CharacterController controller;
    private State currentState;
    public Transform cam;

    private PlayerInputActions inputActions; // 🔹 Input actions referansı
    public float attackCooldown = 2f;
    [HideInInspector] public float lastAttackTime = -Mathf.Infinity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;

        inputActions = new PlayerInputActions();
        inputActions.Enable();

        // 🔹 FSM'e input actions referansını ver
        State.SetInputActions(inputActions);

        currentState = new Grounded(gameObject, anim, controller, cam);
    }

    void Update()
    {
        currentState = currentState.Process();
    }
}
