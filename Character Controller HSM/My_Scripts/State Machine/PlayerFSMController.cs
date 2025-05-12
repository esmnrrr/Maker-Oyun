using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class PlayerFSMController : MonoBehaviour
{
    public Animator anim;
    private CharacterController controller;
    private State currentState;
    public Transform cam;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;
        currentState = new Grounded(gameObject, anim, controller, cam);
        
    }

    void Update()
    {
        currentState = currentState.Process();

    }
}
