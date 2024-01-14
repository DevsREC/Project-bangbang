using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GunSpawner : NetworkBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] List<Transform> spawnLocations; 
    private Queue<Vector3> spawnPoints = new Queue<Vector3>();
    [SerializeField] float SpawnRate = 10f;

    private void Awake()
    {
        InsertIntoQueue();
        //NetworkManager.Singleton.OnServerStarted += StartSpawn;

    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        StartSpawn();
    }
     //NetworkManager.Singleton.OnServerStarted += StartSpawn;

    private void InsertIntoQueue()
    {
        for(int i = 0; i < spawnLocations.Count; i++)
        {
            spawnPoints.Enqueue(spawnLocations[i].position);
        }
    }

    private void StartSpawn()
    {
        if (IsServer)
        {
            Debug.Log("Subscribed");
            SpawnGuns();
        }
        //NetworkManager.Singleton.OnServerStarted -= StartSpawn;
        //StartSpawn();
        //StartCoroutine(DelaySpawner());
    }

    private void SpawnGuns()
    {
        Vector3 temp = spawnPoints.Dequeue();
        GameObject go = Instantiate(prefab, temp, Quaternion.identity);
        spawnPoints.Enqueue(temp);
        go.GetComponent<NetworkObject>().Spawn(true);
        StartCoroutine(DelaySpawner());
        Debug.Log("Spawning");
    }

    IEnumerator DelaySpawner()
    {
        yield return new WaitForSecondsRealtime(SpawnRate);
        SpawnGuns();
        StartCoroutine(DelaySpawner());
    }

    /*
    [ServerRpc(RequireOwnership =false)]

    public void StartSpawnServerRpc()
    {
        Vector3 temp=spawnPoints.Dequeue();
        GameObject go = Instantiate(prefab, temp, Quaternion.identity);
        spawnPoints.Enqueue(temp);
        go.GetComponent<NetworkObject>().Spawn(true);
    }*/


}
