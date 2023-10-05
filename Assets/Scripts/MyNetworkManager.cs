using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MyNetworkManager : NetworkManager
{
    private GameObject[] players;
    int localPlayerIndex;
    private GameObject localPlayer;
    public GameObject FindLocalPlayer()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<PlayerMovement>().IsOwner)
            {
                localPlayerIndex = i;
                break;
            }
        }
        //if this function is called before the local is spawned
        try
        {
            return players[localPlayerIndex];
        }
        catch
        {
            Debug.Log("Player not spawned yet");
            return null;
        }
        
    }

    /*public GameObject FindLocalPlayerBeforeSpawning()
    {
        StartCoroutine(CheckForPlayer());
        Debug.Log("i got called");
        return localPlayer;
    }

    IEnumerator CheckForPlayer()
    {
        //recursive coroutine that runs every 2s until it finds the localplayer
        localPlayer = FindLocalPlayer();
        yield return new WaitForSecondsRealtime(2f);
        if (localPlayer == null)
        {
            StartCoroutine(CheckForPlayer());
        }
        
    }*/
    public void Host()
    {
        StartHost();
    }

    public void Client()
    {
        StartClient();
    }
}
