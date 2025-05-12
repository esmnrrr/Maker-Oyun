using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    private CharacterController controller;
    private float gravity = -200f;
    private float ySpeed = 0f;
    private float groundedThreshold = 0.3f;

    public bool IsGrounded { get; private set; }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        
        IsGrounded = controller.isGrounded || ySpeed < groundedThreshold;

        if (!IsGrounded)
        {
            ySpeed += gravity * Time.deltaTime;  
        }
        else
        {
            
            if (ySpeed < 0)
                ySpeed = -20f;  
        }
    }

    
    public float GetVerticalSpeed()
    {
        return ySpeed;
    }
}
