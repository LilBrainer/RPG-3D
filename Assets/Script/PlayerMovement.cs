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

    Rigidbody playerRb;

    // TESTING

    InputManager inputManager;

    Transform cameraObject;
    Vector3 moveDirection;

    public float movementSpeed = 7;
    public float rotationSpeed = 8;

    //private void Awake()
    //{
    //    input = new PlayerInput();

    //    input.CharacterControls.Movement.performed += ctx =>
    //    {
    //        currentMovement = ctx.ReadValue<Vector2>();
    //        movementPressed = currentMovement.x != 0 || currentMovement.y != 0;
    //    };
    //    input.CharacterControls.Running.performed += ctx => runPressed = ctx.ReadValueAsButton();
    //    input.CharacterControls.Jump.performed += ctx => HandleJump();
    //    }

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerRb = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }

    private void Start()
    {

        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("Walking");
        isRunningHash = Animator.StringToHash("Running");
        groundedHash = Animator.StringToHash("Grounded");
        blendHash = Animator.StringToHash("Velocidad");
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleJump()
    {
        if (animator.GetBool(groundedHash))
        {
            Debug.Log("jump");
            animator.SetTrigger("Jump");
        }
    }

    //void HandleMovement()
    //{
    //    bool isWalking = animator.GetBool(isWalkingHash);
    //    bool isRunning = animator.GetBool(isRunningHash);
    //    //debug.log(currentmovement);
    //    Debug.Log(currentMovement.x + " currentMovemente X");
    //    Debug.Log(currentMovement.y + " currentMovemente Y");

    //    if (movementPressed && !isWalking)
    //    {
    //        animator.SetBool(isWalkingHash, true);
    //        rb.velocity = new Vector3(currentMovement.x, 0, currentMovement.y) * speed;
    //        Debug.Log("mover");
    //    }
        
        
    //    if (!movementPressed && isWalking)
    //    {
    //        animator.SetBool(isWalkingHash, false);
    //    }

    //    if ((movementPressed && runPressed) && !isRunning)
    //    {
    //        animator.SetBool(isRunningHash, true);
    //    }

    //    //Debug.Log("TEST \n movementPressed " + movementPressed + "\nrunPressed " + runPressed + "\nisRunning " + isRunning);
    //    if ((!movementPressed && !runPressed) && isRunning)
    //    {
    //        animator.SetBool(isRunningHash, false);
    //    }


        
    //}

    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();
    }

    public void HandleMovement()
    {
        moveDirection = cameraObject.forward * inputManager.vertiInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
        moveDirection = moveDirection * movementSpeed;

        Vector3 movementVelocity = moveDirection;
        playerRb.velocity = movementVelocity;
    }

    public void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputManager.vertiInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
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
