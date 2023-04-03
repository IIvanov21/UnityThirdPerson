using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.InputReader.CancelTargetEvent += OnCancel;
    }

    public override void Exit()
    {
        stateMachine.InputReader.CancelTargetEvent -= OnCancel;

    }

    public override void Tick(float deltaTime)
    {
        Debug.Log(stateMachine.Targeter.CurrentTarget.name);
    }

    private void OnCancel() 
    {
        stateMachine.Targeter.Cancel();


        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

    
}
