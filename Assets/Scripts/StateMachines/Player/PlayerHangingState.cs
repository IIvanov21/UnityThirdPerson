using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHangingState : PlayerBaseState
{
    private readonly int HangingIdleHash = Animator.StringToHash("HangingIdle");
    private const float CrossFixedTime = 0.1f;
    Vector3 closestPoint;
    Vector3 ledgeForward;

    public PlayerHangingState(PlayerStateMachine stateMachine,  Vector3 ledgeForward,Vector3 closestPoint) : base(stateMachine)
    {
        this.ledgeForward = ledgeForward;
        this.closestPoint = closestPoint;
    }

    public override void Enter()
    {
        stateMachine.transform.rotation = Quaternion.LookRotation(ledgeForward, Vector3.up);

        stateMachine.Controller.enabled = false;
        stateMachine.transform.position = closestPoint - (stateMachine.LedgeDetector.transform.position - stateMachine.transform.position);
        stateMachine.Controller.enabled = true;

        stateMachine.Animator.CrossFadeInFixedTime(HangingIdleHash, CrossFixedTime);
    }

    public override void Tick(float deltaTime)
    {
        if(stateMachine.InputReader.MovementValue.y <0f)
        {
            stateMachine.Controller.Move(Vector3.zero);
            stateMachine.ForceReceiver.Reset();
            stateMachine.SwitchState(new PlayerFallingState(stateMachine));
        }
        
        if (stateMachine.InputReader.MovementValue.y > 0f)
        {
         
            stateMachine.SwitchState(new PlayerPullUpState(stateMachine));
        }

    }

    public override void Exit()
    {

    }

    
}
