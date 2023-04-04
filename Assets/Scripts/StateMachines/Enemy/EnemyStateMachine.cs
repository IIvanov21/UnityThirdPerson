using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyStateMachine : StateMachine
{
    [field: SerializeField]public Animator Animator { get; private set; }
    [field: SerializeField] public float PlayerChasingRange { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public NavMeshAgent Agent { get; private set; }
    [field: SerializeField] public float MovementSpeed { get; private set; }



    public GameObject Player { get; private set; }

    void Start()
    {
        Player = GameObject.FindWithTag("Player");

        SwitchState(new EnemyIdleState(this));

        Agent.updatePosition = false;
        Agent.updateRotation = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerChasingRange);

    }

}
