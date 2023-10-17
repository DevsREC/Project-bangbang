using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    private void Awake()
    {
        //MyNetworkManager myNetworkManager = FindObjectOfType<MyNetworkManager>().GetComponent<MyNetworkManager>();
        //myNetworkManager.onServerInitiated += StartSpawn;
        NetworkManager.Singleton.OnServerStarted += StartSpawn;
    }

    private void StartSpawn()
    {
        StartSpawnServerRpc();
        //return null;
    }

    [ServerRpc(RequireOwnership =false)]

    public void StartSpawnServerRpc()
    {
        GameObject go = Instantiate(prefab, transform.position, transform.rotation);
        go.GetComponent<NetworkObject>().Spawn();
    }

}
