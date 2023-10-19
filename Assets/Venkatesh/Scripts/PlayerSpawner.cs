using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class PlayerSpawner : MonoSingleton<PlayerSpawner>
{
    [SerializeField] private GameObject prefab;
    [SerializeField] List<Transform> spawnLocations;
    private Queue<Vector3> spawnPoints;
    [SerializeField] float spawnDelay = 1f;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        InsertIntoQueue();
    }
    public void StartSpawn(ulong clientId)
    {
        Debug.Log("Spawn is initiated at startSpawn");
        Vector3 temp = spawnPoints.Dequeue();
        StartCoroutine(DelaySpawn(spawnDelay , temp , Quaternion.identity , clientId));
        spawnPoints.Enqueue(temp);
    }

    IEnumerator DelaySpawn(float delay , Vector3 position, Quaternion rotation, ulong clientId)
    {
        Debug.Log("Spawn initiated at delay spawn");
        yield return new WaitForSecondsRealtime(delay);
        Spawn(position, rotation, clientId);
    }

    private void Spawn(Vector3 position, Quaternion rotation, ulong clientId)
    {
        Debug.Log("player spawn initiated");
        prefab.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId , true);
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
