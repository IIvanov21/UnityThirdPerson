using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    // Store the player's movement input as a Vector2.
    public Vector2 MovementValue { get; private set; }
    // Store whether the player is attacking or not.
    public bool IsAttacking { get; private set; }
    // Store whether the player is blocking or not.
    public bool IsBlocking { get; private set; }

    // Define events that other scripts can subscribe to.
    public event Action JumpEvent;
    public event Action DodgeEvent;
    public event Action TargetEvent;

    // Store the Input System controls.
    private Controls controls;

    // Initialize when the script starts.
    private void Start()
    {
        // Create a new instance of the Controls class.
        controls = new Controls();
        // Set the callbacks for player actions to this script.
        controls.Player.SetCallbacks(this);
        // Enable the player input.
        controls.Player.Enable();
    }

    // Clean up when the script is destroyed.
    private void OnDestroy()
    {
        // Disable the player input.
        controls.Player.Disable();
    }

    // Triggered when the jump input is performed.
    public void OnJump(InputAction.CallbackContext context)
    {
        // If the input is not performed, do nothing.
        if (!context.performed) { return; }
        // Invoke the JumpEvent, notifying subscribers.
        JumpEvent?.Invoke();
    }

    // Triggered when the dodge input is performed.
    public void OnDodge(InputAction.CallbackContext context)
    {
        // If the input is not performed, do nothing.
        if (!context.performed) { return; };
        // Invoke the DodgeEvent, notifying subscribers.
        DodgeEvent?.Invoke();
    }

    // Triggered when the movement input is changed.
    public void OnMove(InputAction.CallbackContext context)
    {
        // Read the value of the movement input and store it.
        MovementValue = context.ReadValue<Vector2>();
        // Output the movement value to the console for debugging.
        Debug.Log(MovementValue);
    }

    // Triggered when the look input is changed.
    public void OnLook(InputAction.CallbackContext context)
    {
        // This method is currently empty.
        // You can implement it if needed for camera control.
    }

    // Triggered when the target input is performed.
    public void OnTarget(InputAction.CallbackContext context)
    {
        // If the input is not performed, do nothing.
        if (!context.performed) { return; };
        // Invoke the TargetEvent, notifying subscribers.
        TargetEvent?.Invoke();
    }

    // Triggered when the attack input is performed or canceled.
    public void OnAttack(InputAction.CallbackContext context)
    {
        // If the input is performed, set IsAttacking to true.
        if (context.performed)
        {
            IsAttacking = true;
        }
        // If the input is canceled, set IsAttacking to false.
        else if (context.canceled)
        {
            IsAttacking = false;
        }
    }

    // Triggered when the block input is performed or canceled.
    public void OnBlock(InputAction.CallbackContext context)
    {
        // If the input is performed, set IsBlocking to true.
        if (context.performed)
        {
            IsBlocking = true;
        }
        // If the input is canceled, set IsBlocking to false.
        else if (context.canceled)
        {
            IsBlocking = false;
        }
    }
}