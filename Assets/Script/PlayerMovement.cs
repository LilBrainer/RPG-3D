using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Animator animator;

    int isWalkingHash;
    int isRunningHash;
    int blendHash;

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
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("Walking");
        isRunningHash = Animator.StringToHash("Running");
        blendHash = Animator.StringToHash("Velocidad");
    }

    private void Update()
    {
        handleMovement();
    }


    void handleMovement()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);

        if (movementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
            animator.SetFloat(blendHash, currentMovement.x);
            rb.velocity = currentMovement;
        }
        
        if (!movementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if ((movementPressed && runPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }

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
}
