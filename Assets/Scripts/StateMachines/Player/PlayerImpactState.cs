using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImpactState : PlayerBaseState
{
    private readonly int ImpactHash = Animator.StringToHash("Impact");
    private readonly float AnimationDamp = 0.1f;
    private readonly float CrossFadeTime = 0.1f;
    private float duration = 1.0f;
    public PlayerImpactState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeTime);

    }
    
    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        duration -= deltaTime;

        if(duration <= 0.0f)
        {
            ReturnToLocomotion();
        }
    }

    public override void Exit()
    {
    }

}
