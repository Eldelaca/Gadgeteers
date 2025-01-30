using System;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    private CharacterController controller;
    [SerializeField] private Transform camera;
    
    [Header("MovementSettings")]
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float turnSpeed = 2f;
    [SerializeField] private float gravity = -9.81f; // Earth Like Gravity, change for future powerups (Jet shoes?)
    [SerializeField] private float jumpHeight = 2f;
    
    private float verticalVelocity;
    
    [Header("Input")] 
    private float moveInput; 
    private float turnInput;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        InputManagement();
        Movement();
    }

    // @Albus Function isn't completely redundant, using it for easy updates and cleanliness.
    private void Movement()
    {
        GroundManager();
        TurnControl();
    }

    private void GroundManager()
    {
        Vector3 move = new Vector3(turnInput, 0f, moveInput);
        move = camera.transform.TransformDirection(move);

        move *= moveSpeed;

        move.y = VerticalForceCalculation();
        
        controller.Move(move * Time.deltaTime);

    }

    private void TurnControl()
    {
        if (Mathf.Abs(turnInput) < 0 || Mathf.Abs(moveInput) > 0)
        {
            Vector3 currentRotation = controller.velocity.normalized;
            currentRotation.y = 0;
            
            currentRotation.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(currentRotation);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }

    private float VerticalForceCalculation()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = gravity * 0.01f;

            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * gravity * 2.0f);
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        return verticalVelocity;
    }
    

    private void InputManagement()
    {
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }
    
}
