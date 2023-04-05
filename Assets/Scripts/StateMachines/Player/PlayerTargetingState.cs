using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    private readonly int TargetingBlendTree = Animator.StringToHash("TargetingBlendTree");
    private readonly int TargetingForward= Animator.StringToHash("TargetingForward");
    private readonly int TargetingRight = Animator.StringToHash("TargetingRight");
    private const float AnimatorDampTime = 0.1f;

    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTree,AnimatorDampTime);
        stateMachine.InputReader.CancelTargetEvent += OnCancel;
    }

    public override void Exit()
    {
        stateMachine.InputReader.CancelTargetEvent -= OnCancel;

    }

    public override void Tick(float deltaTime)
    {

        if (stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine,0));
            return;
        }

        if(stateMachine.InputReader.IsBlocking)
        {
            stateMachine.SwitchState(new PlayerBlockingState(stateMachine));
            return;
        }

        if(stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }

        Vector3 movement = CalculateMovement();

        Move(movement * stateMachine.TargetingMovementSpeed, deltaTime);


        FaceTarget();

        UpdateAnimator(deltaTime);
    }

    private void OnCancel() 
    {
        stateMachine.Targeter.Cancel();


        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

    private Vector3 CalculateMovement()
    {
        Vector3 movement=new Vector3();

        movement += stateMachine.transform.right*stateMachine.InputReader.MovementValue.x;
        movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;

        return movement;
    }

    private void UpdateAnimator(float deltaTime)
    {
        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(TargetingRight, 0, AnimatorDampTime, deltaTime);
            stateMachine.Animator.SetFloat(TargetingForward, 0, AnimatorDampTime, deltaTime);
            return;
        }

        stateMachine.Animator.SetFloat(TargetingForward, stateMachine.InputReader.MovementValue.y, AnimatorDampTime, deltaTime);
        stateMachine.Animator.SetFloat(TargetingRight, stateMachine.InputReader.MovementValue.x, AnimatorDampTime, deltaTime);

    }
}
