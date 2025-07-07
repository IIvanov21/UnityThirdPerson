using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;

public class NetworkObjectSetup : NetworkBehaviour
{
    [SerializeField] InputReader inputReader;
    [SerializeField] CharacterController characterController;
    [SerializeField] PlayerStateMachine playerStateMachine;
    [SerializeField] Camera mainCamera;
    [SerializeField] CinemachineStateDrivenCamera stateCamera;
    [SerializeField] CinemachineFreeLook freeCamera;
    [SerializeField] CinemachineVirtualCamera targetCamera;
    [SerializeField] AudioListener audioListener;
    [SerializeField] GameObject playerRig;

    public override void OnNetworkSpawn()
    {
        audioListener.enabled = IsOwner;
        inputReader.enabled = IsOwner;
        characterController.enabled = IsOwner;
        playerStateMachine.enabled = IsOwner;
        mainCamera.enabled = IsOwner;
        freeCamera.enabled = IsOwner;
        targetCamera.enabled = IsOwner;
        stateCamera.enabled = IsOwner;

        base.OnNetworkSpawn();
        //Set Spawning point here
        if (IsOwner)
        {
            Vector3 spawnPoint = SpawnManager.Instance.GetSpawnPoint();
            playerRig.transform.position = spawnPoint;
            Debug.Log(spawnPoint);
        }
    }

    public override void OnNetworkDespawn()
    {
        audioListener.enabled = false;
        playerStateMachine.enabled = false;
        characterController.enabled = false;
        stateCamera.enabled = false;
        freeCamera.enabled = false;
        targetCamera.enabled = false;
        mainCamera.enabled = false;
        base.OnNetworkDespawn();
    }

    
}
