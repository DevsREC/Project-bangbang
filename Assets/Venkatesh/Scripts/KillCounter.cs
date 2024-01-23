using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
public class KillCounter : NetworkBehaviour
{
    [SerializeField]public List<List<int>> playersKillDeathTable = new List<List<int>>();


    private void Start()
    {
        AssignRemainingPlayersInTable();
        if (NetworkManager.Singleton == null)
        {
            Debug.Log("networkmanager is null");
        }
        else
        {
            NetworkManager.Singleton.OnClientConnectedCallback += UpdatePlayersList;
        }
        
    }

    private void AssignFirstPlayerInTable()
    {
        List<int> temp1 = new List<int>();
        temp1.Add(0);
        temp1.Add(0);
        temp1.Add(0);
        playersKillDeathTable.Add(temp1);
        temp1 = null;
        AssignRemainingPlayersInTable();

    }

    private void AssignRemainingPlayersInTable()
    {
        if (IsServer)
        {
            foreach (ulong i in NetworkManager.Singleton.ConnectedClientsIds)
            {
                UpdatePlayersList(i);
            }
        }

    }
    /*private void Update()
    {
        if (IsServer) 
        {
            Debug.Log(playersKillDeathTable[0][0] + " " + playersKillDeathTable[0][1] + " " + playersKillDeathTable[0][2]);
            Debug.Log(playersKillDeathTable[1][0] + " " + playersKillDeathTable[1][1] + " " + playersKillDeathTable[1][2]);
        }
            
    }*/

    private void UpdatePlayersList(ulong a)
    {
        if (IsHost)
        {
            AddPlayerToPlayersTableClientRpc(a);
        }
        
    }

    public void UpdateKillCount(ulong clientId)
    {
        //playersKillDeathTable[(int)clientId][1]++;
        UpdatePlayersKillTableClientRpc(clientId);
    }

    public void UpdateDeathCount(ulong clientId)
    {
        //playersKillDeathTable[(int)clientId][2]++;
        UpdatePlayersDeathTableClientRpc(clientId);
    }

    [ClientRpc]

    private void UpdatePlayersKillTableClientRpc(ulong clientId)
    {
        playersKillDeathTable[(int)clientId][1]++;
    }

    [ClientRpc]
    private void UpdatePlayersDeathTableClientRpc(ulong clientId)
    {
        playersKillDeathTable[(int)clientId][2]++;
    }

    [ClientRpc]
    private void AddPlayerToPlayersTableClientRpc(ulong clientId)
    {
        List<int> temp = new List<int>();
        temp.Add((int)clientId);
        temp.Add(0);
        temp.Add(0);
        playersKillDeathTable.Add(temp);
    }

    //testing purpose

    public void Print()
    {
        Debug.Log(playersKillDeathTable[0][0] + " " + playersKillDeathTable[0][1] + " " + playersKillDeathTable[0][2]);
        Debug.Log(playersKillDeathTable[1][0] + " " + playersKillDeathTable[1][1] + " " + playersKillDeathTable[1][2]);

    }

}
