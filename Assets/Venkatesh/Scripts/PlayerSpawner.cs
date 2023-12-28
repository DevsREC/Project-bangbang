using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class PlayerSpawner : MonoSingleton<PlayerSpawner>
{
    //[SerializeField] private GameObject prefab;
    PlayerSpawnerNetwork playerSpawnerNetwork;
    [SerializeField] List<Transform> spawnLocations;
    private Queue<Vector3> spawnPoints;
    [SerializeField] float spawnDelay = 5f;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        playerSpawnerNetwork = FindObjectOfType<PlayerSpawnerNetwork>();
        InsertIntoQueue();
    }
    public void StartSpawn(GameObject player ,  ulong clientId)
    {
        Debug.Log("Spawn is initiated at startSpawn");
        Vector3 temp = spawnPoints.Dequeue();
        StartCoroutine(DelaySpawn(player , spawnDelay , temp , Quaternion.identity , clientId));
        spawnPoints.Enqueue(temp);
    }

    IEnumerator DelaySpawn(GameObject player , float delay , Vector3 position, Quaternion rotation, ulong clientId)
    {
        Debug.Log("Spawn initiated at delay spawn");
        yield return new WaitForSecondsRealtime(delay);
        Spawn(player , position, rotation, clientId);
    }

    private void Spawn(GameObject player ,Vector3 position, Quaternion rotation, ulong clientId)
    {
        Debug.Log("player spawn initiated");
        player.transform.position = position;
        playerSpawnerNetwork.PlayerSwitch(player);
        playerSpawnerNetwork.PlayerSpawnClientRpc();       
        //player.SetActive(true);
        //prefab.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId , true);
    }

    public void PlayerDespawn(GameObject player)
    {
        playerSpawnerNetwork.PlayerSwitch(player);
        playerSpawnerNetwork.DestroyPlayerClientRpc();
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
