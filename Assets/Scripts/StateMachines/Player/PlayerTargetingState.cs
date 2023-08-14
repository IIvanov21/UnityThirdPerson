using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    // Hashes for Animator parameters
    private readonly int TargetingBlendTree = Animator.StringToHash("TargetingBlendTree");
    private readonly int TargetingForward = Animator.StringToHash("TargetingForward");
    private readonly int TargetingRight = Animator.StringToHash("TargetingRight");

    private const float AnimatorDampTime = 0.1f;

    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        // Crossfade to targeting animation
        stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTree, AnimatorDampTime);

        // Subscribe to input events
        stateMachine.InputReader.TargetEvent += OnTarget;
        stateMachine.InputReader.DodgeEvent += OnDodge;
        stateMachine.InputReader.JumpEvent += OnJump;
    }

    public override void Exit()
    {
        // Unsubscribe from input events
        stateMachine.InputReader.TargetEvent -= OnTarget;
        stateMachine.InputReader.DodgeEvent -= OnDodge;
        stateMachine.InputReader.JumpEvent -= OnJump;
    }

    public override void Tick(float deltaTime)
    {
        // Handle state transitions based on input and conditions

        if (stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }

        if (stateMachine.InputReader.IsBlocking)
        {
            stateMachine.SwitchState(new PlayerBlockingState(stateMachine));
            return;
        }

        if (stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }

        // Calculate movement
        Vector3 movement = CalculateMovement(deltaTime);

        // Move the player
        Move(movement * stateMachine.TargetingMovementSpeed, deltaTime);

        // Orient the player towards the target
        FaceTarget();

        // Update Animator parameters
        UpdateAnimator(deltaTime);
    }

    private void OnTarget()
    {
        // Cancel targeting and switch to free look state
        stateMachine.Targeter.Cancel();
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

    private Vector3 CalculateMovement(float deltaTime)
    {
        // Calculate movement based on input
        Vector3 movement = Vector3.zero;

        movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x;
        movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;

        return movement;
    }

    private void UpdateAnimator(float deltaTime)
    {
        // Update Animator parameters for movement
        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(TargetingRight, 0, AnimatorDampTime, deltaTime);
            stateMachine.Animator.SetFloat(TargetingForward, 0, AnimatorDampTime, deltaTime);
            return;
        }

        stateMachine.Animator.SetFloat(TargetingForward, stateMachine.InputReader.MovementValue.y, AnimatorDampTime, deltaTime);
        stateMachine.Animator.SetFloat(TargetingRight, stateMachine.InputReader.MovementValue.x, AnimatorDampTime, deltaTime);
    }

    private void OnDodge()
    {
        // Switch to dodging state
        stateMachine.SwitchState(new PlayerDodgingState(stateMachine, stateMachine.InputReader.MovementValue));
    }

    private void OnJump()
    {
        // Switch to jumping state
        stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
    }
}