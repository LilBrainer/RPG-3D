using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerInput playerInput;

    public Vector2 movementInput;
    public float vertiInput;
    public float horizInput;

    private void OnEnable()
    {
        if (playerInput == null)
        {
            playerInput = new PlayerInput();


            playerInput.CharacterControls.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        }

        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        //HandleJumpingInput();
        //HandleActionInput();
    }

    private void HandleMovementInput()
    {
        vertiInput = movementInput.y;
        horizInput = movementInput.x;
    }
}
