using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine;

    // Constructor that takes a PlayerStateMachine instance
    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    // Move the player based on the motion vector and delta time
    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    // Move the player with zero motion vector and delta time
    protected void Move(float deltaTime)
    {
        stateMachine.Controller.Move((Vector3.zero + stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    // Rotate the player to face the current target (if any)
    protected void FaceTarget()
    {
        if (stateMachine.Targeter.CurrentTarget == null) return;

        Vector3 facingVector = stateMachine.Targeter.CurrentTarget.transform.position - stateMachine.transform.position;

        // Set the y-component to zero to ensure rotation is only in the horizontal plane
        facingVector.y = 0.0f;

        stateMachine.transform.rotation = Quaternion.LookRotation(facingVector);
    }

    // Switch to the appropriate state based on the presence of a target
    protected void ReturnToLocomotion()
    {
        if (stateMachine.Targeter.CurrentTarget != null)
        {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        }
        else
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }
    }
}