using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    [SerializeField] Transform[] spawnPoints;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    public Vector3 GetSpawnPoint()
    {
        int index=Random.Range(0, spawnPoints.Length);

        Vector3 spawnPoint=spawnPoints[index].position;

        return spawnPoint;
    }
}
