using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class represents the falling state of the player character.
public class PlayerFallingState : PlayerBaseState
{
    // Hash value for the "Fall" animation state.
    private readonly int FallHash = Animator.StringToHash("Fall");

    // Duration of the crossfade animation transition.
    private const float CrossFixedTime = 0.1f;

    // Store the momentum of the player character.
    private Vector3 momentum;

    // Constructor for the falling state.
    public PlayerFallingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    // Called when entering the falling state.
    public override void Enter()
    {
        // Capture the player's horizontal momentum and reset vertical momentum.
        momentum = stateMachine.Controller.velocity;
        momentum.y = 0.0f;

        // Initiate a smooth crossfade animation to the "Fall" state.
        stateMachine.Animator.CrossFadeInFixedTime(FallHash, CrossFixedTime);

        // Subscribe to the ledge detection event.
        stateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;
    }

    // Called every frame while in the falling state.
    public override void Tick(float deltaTime)
    {
        // Move the player character using the captured momentum.
        Move(momentum, deltaTime);

        // If the player character is grounded, transition back to locomotion state.
        if (stateMachine.Controller.isGrounded)
        {
            ReturnToLocomotion();
        }

        // Ensure the player character is facing the intended direction.
        FaceTarget();
    }

    // Called when exiting the falling state.
    public override void Exit()
    {
        // Unsubscribe from the ledge detection event.
        stateMachine.LedgeDetector.OnLedgeDetect -= HandleLedgeDetect;
    }

    // Event handler for ledge detection.
    private void HandleLedgeDetect(Vector3 ledgeForward, Vector3 closestPoint)
    {
        // Transition to the hanging state with information about the detected ledge.
        stateMachine.SwitchState(new PlayerHangingState(stateMachine, ledgeForward, closestPoint));
    }
}