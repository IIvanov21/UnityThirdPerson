using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private Attack attack;
    private float previousFrameTime;
    private bool alreadyAppliedForce = false;
    public PlayerAttackingState(PlayerStateMachine stateMachine,int attackIndex) : base(stateMachine)
    {
        attack = stateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
        stateMachine.WeaponDamage.SetAttack(attack.Damage,attack.Knockback);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        FaceTarget();

        float normalisedTime=GetNormalisedTime(stateMachine.Animator,"Attack");
        if (normalisedTime >= previousFrameTime && normalisedTime < 1f)
        {
            if (normalisedTime >= attack.ForceTime)
            {
                TryApplyForce();
            }
            if (stateMachine.InputReader.IsAttacking)
            {
                TryComboAttack(normalisedTime);
            }
        }
        else
        {
            if (stateMachine.Targeter.CurrentTarget != null) stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            else stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }

        previousFrameTime=normalisedTime;

        
    }

    

    public override void Exit()
    {

    }

    private void TryComboAttack(float normalisedTime)
    {
        if (attack.ComboStateIndex == -1) return;

        if (normalisedTime < attack.ComboAttackTime) return;

        stateMachine.SwitchState
            (
            new PlayerAttackingState
                (
                    stateMachine,
                    attack.ComboStateIndex
                )
            );
    }

  

    private void TryApplyForce()
    {
        if (alreadyAppliedForce) return;
        stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward*attack.Force);
        alreadyAppliedForce=true;
    }
}
