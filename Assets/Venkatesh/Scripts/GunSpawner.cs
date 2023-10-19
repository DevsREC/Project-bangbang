using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GunSpawner : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] List<Transform> spawnLocations; 
    private Queue<Vector3> spawnPoints = new Queue<Vector3>();
    [SerializeField] float SpawnRate = 10f;

    private void Awake()
    {
        InsertIntoQueue();
        
    }

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += StartSpawn;
    }

    private void InsertIntoQueue()
    {
        for(int i = 0; i < spawnLocations.Count; i++)
        {
            spawnPoints.Enqueue(spawnLocations[i].position);
        }
    }

    private void StartSpawn()
    {
        NetworkManager.Singleton.OnServerStarted -= StartSpawn;
        StartSpawnServerRpc();
        StartCoroutine(DelaySpawner());
    }

    IEnumerator DelaySpawner()
    {
        yield return new WaitForSecondsRealtime(SpawnRate);
        StartSpawnServerRpc();
        StartCoroutine(DelaySpawner());
    }

    [ServerRpc(RequireOwnership =false)]

    public void StartSpawnServerRpc()
    {
        Vector3 temp=spawnPoints.Dequeue();
        GameObject go = Instantiate(prefab, temp, Quaternion.identity);
        spawnPoints.Enqueue(temp);
        go.GetComponent<NetworkObject>().Spawn(true);
    }


}
