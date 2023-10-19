using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] float SpawnRate = 10f;

    private void Awake()
    {
        NetworkManager.Singleton.OnServerStarted += StartSpawn;
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
        GameObject go = Instantiate(prefab, transform.position, transform.rotation);
        go.GetComponent<NetworkObject>().Spawn(true);
    }


}
