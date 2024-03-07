using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Animator animator;

    int isWalkingHash;
    int isRunningHash;
    int groundedHash;
    int blendHash;
    int speed = 2;

    PlayerInput input;

    Vector2 currentMovement;
    bool movementPressed;
    bool runPressed;

    Rigidbody rb;

    private void Awake()
    {
        input = new PlayerInput();

        input.CharacterControls.Movement.performed += ctx =>
        {
            currentMovement = ctx.ReadValue<Vector2>();
            movementPressed = currentMovement.x != 0 || currentMovement.y != 0;
        };
        input.CharacterControls.Running.performed += ctx => runPressed = ctx.ReadValueAsButton();
        input.CharacterControls.Jump.performed += ctx => HandleJump();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("Walking");
        isRunningHash = Animator.StringToHash("Running");
        groundedHash = Animator.StringToHash("Grounded");
        blendHash = Animator.StringToHash("Velocidad");
    }

    private void Update()
    {
        HandleMovement();
        //Debug.Log("correr "+runPressed);
    }

    private void HandleJump()
    {
        if (animator.GetBool(groundedHash))
        {
            Debug.Log("jump");
            animator.SetTrigger("Jump");
        }
    }

    void HandleMovement()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);
        //debug.log(currentmovement);
        Debug.Log(currentMovement.x + " currentMovemente X");
        Debug.Log(currentMovement.y + " currentMovemente Y");

        if (movementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
            rb.velocity = new Vector3(currentMovement.x, 0, currentMovement.y) * speed;
            Debug.Log("mover");
        }
        
        
        if (!movementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if ((movementPressed && runPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }

        //Debug.Log("TEST \n movementPressed " + movementPressed + "\nrunPressed " + runPressed + "\nisRunning " + isRunning);
        if ((!movementPressed && !runPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }


        
    }

    private void OnEnable()
    {
        input.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        input.CharacterControls.Disable();
    }

    private void OnCollisionEnter(Collision collision)
    {
        animator.SetBool(groundedHash, true);
        if (animator.GetBool("Falling") == true)
        {
            animator.SetBool("Falling", false);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        animator.SetBool(groundedHash, false);
    }
}
