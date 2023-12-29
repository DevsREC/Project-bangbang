using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class PlayerSpawner : NetworkBehaviour //, MonoSingleton<PlayerSpawner>
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
        GameObject player = FindObjectOfType<MyNetworkManager>().FindListOfPlayers()[clientId];
        
        //playerSpawnerNetwork.PlayerSwitch(player);
        PlayerSpawnClientRpc(position , clientId);       
        //player.SetActive(true);
        //prefab.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId , true);
    }

    [ClientRpc]
    public void PlayerSpawnClientRpc(Vector3 position , ulong clientId)
    {
        DoThisToSpawnPlayer(position , clientId);
        //player.SetActive(true);
    }

    private void DoThisToSpawnPlayer(Vector3 position, ulong clientId)
    {
        GameObject player = FindObjectOfType<MyNetworkManager>().FindListOfPlayers()[clientId];
        player.transform.position = position;
        player.GetComponent<BoxCollider2D>().isTrigger = false;
        player.GetComponent<PlayerMovement>().enabled = true;
        //player.GetComponent<Rigidbody2D>().gravityScale = 1;
        player.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
        player.GetComponent<Health>().RestoreHealth();
    }

    public void PlayerDespawn(ulong clientId)
    {
        //playerSpawnerNetwork.PlayerSwitch(player);
        DestroyPlayerClientRpc(clientId);
    }

    [ClientRpc]
    public void DestroyPlayerClientRpc(ulong clientId)
    {
        GameObject player = FindObjectOfType<MyNetworkManager>().FindListOfPlayers()[clientId];
        if (player == null)
        {
            return;
        }
        player.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
        player.GetComponent<BoxCollider2D>().isTrigger = true;
        player.GetComponent<PlayerMovement>().enabled = false;
        //player.GetComponent<Rigidbody2D>().gravityScale = 0;
        //gameObject.SetActive(false);
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
