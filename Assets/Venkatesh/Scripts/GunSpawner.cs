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

    /*private void Awake()
    {
        

        //NetworkManager.Singleton.OnServerStarted += StartSpawn;

    }*/
    private void Awake()
    {
        InsertIntoQueue();
        

    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        StartSpawn();
        /*if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_onLoadEventCompleted;
        }*/

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
            //gameObject.GetComponent<NetworkObject>().Spawn();
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

    private void SceneManager_onLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode
        , List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        SpawnGuns();
    }

    IEnumerator DelaySpawner()
    {
        yield return new WaitForSecondsRealtime(SpawnRate);
        SpawnGuns();
        //StartCoroutine(DelaySpawner());
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
