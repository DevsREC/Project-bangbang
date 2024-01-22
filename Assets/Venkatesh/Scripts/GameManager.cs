using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class GameManager : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject[] Prefabs;
    /*[SerializeField] private GameObject gameTimerPrefab;
    [SerializeField] private GameObject gunSpawnerPrefab;
    [SerializeField] private GameObject playerSpawnerPrefab;*/

    /*private void Awake()
    {
        gameObject.GetComponent<NetworkObject>().Spawn();
    }*/
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_onLoadEventCompleted;
        }

    }

    private void SceneManager_onLoadEventCompleted(string sceneName , UnityEngine.SceneManagement.LoadSceneMode loadSceneMode 
        ,List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            GameObject player = Instantiate(playerPrefab);
            player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
        }
        for(int i=0; i < Prefabs.Length; i++)
        {
            GameObject go = Instantiate(Prefabs[i]);
            go.GetComponent<NetworkObject>().Spawn();
        }
    }
}
