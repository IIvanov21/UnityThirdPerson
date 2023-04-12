using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField]public InputReader InputReader{get; private set;}
    [field: SerializeField]public CharacterController Controller { get; private set;}
    [field: SerializeField]public Animator Animator { get; private set; }
    [field: SerializeField] public Attack[] Attacks { get; private set; }
    [field: SerializeField] public Targeter Targeter { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public WeaponDamage WeaponDamage { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public Ragdoll Ragdoll { get; private set; }
    [field: SerializeField] public LedgeDetector LedgeDetector { get; private set; }



    [field: SerializeField] public int AttackDamage { get; private set; }
    [field: SerializeField] public float RotationDamping { get; private set; }
    [field: SerializeField] public float TargetingMovementSpeed { get; private set; }
    [field: SerializeField] public float FreeLookMovementSpeed { get; private set;}
    [field: SerializeField] public float Knockback { get; private set; }
    [field: SerializeField] public float DodgeDurtaion { get; private set; }
    [field: SerializeField] public float DodgeLength { get; private set; }
    [field: SerializeField] public float PreviousDodgeTime { get; private set; } = Mathf.NegativeInfinity;
    [field: SerializeField] public float DodgeCoolDown { get; private set; }
    [field: SerializeField] public float JumpForce { get; private set; }






    public Vector3 MovementVector;
    public Transform MainCameraTransform { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        MainCameraTransform = Camera.main.transform;

        SwitchState(new PlayerFreeLookState(this));

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        Health.OnTakeDamage += HandleTakeDamage;
        Health.OnDie += HandleDieEvent;
    }

    private void OnDisable()
    {
        Health.OnTakeDamage -= HandleTakeDamage;
        Health.OnDie -= HandleDieEvent;

    }

    private void HandleTakeDamage()
    {
        SwitchState(new PlayerImpactState(this));
    }

    private void HandleDieEvent()
    {
        SwitchState(new PlayerDeadState(this));
    }

    public void SetDodgeTime(float time)
    {
        PreviousDodgeTime = time;
    }

    


}
