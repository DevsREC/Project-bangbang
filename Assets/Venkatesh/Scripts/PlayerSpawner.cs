using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
using System;
public class PlayerSpawner : NetworkBehaviour //, MonoSingleton<PlayerSpawner>
{
    [SerializeField] List<Transform> spawnLocations;
    private Queue<Vector3> spawnPoints;
    [SerializeField] float spawnDelay = 5f;
    public event PlayerDespawnDelegate onPlayerDespawn;
    public event PlayerSpawnDelegate onPlayerSpawn;

    public delegate void PlayerDespawnDelegate(ulong clientId);
    public delegate void PlayerSpawnDelegate(Vector3 position , ulong clientId);
    private void Awake()
    {
        DontDestroyOnLoad(this);
        InsertIntoQueue();
    }
    public void StartSpawn(ulong clientId)
    {
        Vector3 temp = spawnPoints.Dequeue();
        StartCoroutine(DelaySpawn(spawnDelay , temp , Quaternion.identity , clientId));
        spawnPoints.Enqueue(temp);
    }

    IEnumerator DelaySpawn(float delay , Vector3 position, Quaternion rotation, ulong clientId)
    {
        yield return new WaitForSecondsRealtime(delay);
        Spawn(position, rotation, clientId);
    }

    private void Spawn(Vector3 position, Quaternion rotation, ulong clientId)
    {
        PlayerSpawnClientRpc(position , clientId);       

    }

    [ClientRpc]
    public void PlayerSpawnClientRpc(Vector3 position , ulong clientId)
    {
        onPlayerSpawn?.Invoke(position, clientId);
    }

    public void PlayerDespawn(ulong clientId)
    {
        PlayerDespawnClientRpc(clientId);
    }

    [ClientRpc]
    public void PlayerDespawnClientRpc(ulong clientId)
    {
        onPlayerDespawn?.Invoke(clientId);
    }

    private void InsertIntoQueue()
    {
        for (int i = 0; i < spawnLocations.Count; i++)
        {
            spawnPoints = new Queue<Vector3>();
            spawnPoints.Enqueue(spawnLocations[i].position);
        }
    }

}
