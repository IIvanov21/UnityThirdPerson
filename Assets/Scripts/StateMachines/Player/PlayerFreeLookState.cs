using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    private readonly int FreeLookBlendTree = Animator.StringToHash("FreeLookBlendTree");

    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine){    }

    private const float AnimatorDampTime = 0.1f;

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTree,AnimatorDampTime);
        stateMachine.InputReader.TargetEvent += OnTarget;
    }

    

    public override void Tick(float deltaTime)
    {
        if (stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine,0));
            return;
        }

        stateMachine.MovementVector = CalculateMovement();
        

        Move(stateMachine.MovementVector*stateMachine.FreeLookMovementSpeed,deltaTime);

        FaceMovementDirection(deltaTime);
    }

    public override void Exit()
    {
        stateMachine.InputReader.TargetEvent -= OnTarget;

    }

    private Vector3 CalculateMovement()
    {

        Vector3 cameraForward= stateMachine.MainCameraTransform.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 cameraRight= stateMachine.MainCameraTransform.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        return cameraForward*stateMachine.InputReader.MovementValue.y + cameraRight*stateMachine.InputReader.MovementValue.x;

    }

    private void FaceMovementDirection(float deltaTime)
    {
        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
            return;
        }

        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1, AnimatorDampTime, deltaTime);

        stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation,
            Quaternion.LookRotation(stateMachine.MovementVector),deltaTime*stateMachine.RotationDamping);
    }
   
    private void OnTarget()
    {
        if (!stateMachine.Targeter.SelectTarget()) return;
        stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
    }

    
}
