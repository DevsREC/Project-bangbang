using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class UIManager : MonoBehaviour
{
    private GameObject localPlayer;
    private GunHandeler gunHandeler;
    [SerializeField] private GameObject swapButton;
    /*private void AssignLocalPlayer()
    {   //Assigns localplayer and gunhandeler
        localPlayer = FindObjectOfType<MyNetworkManager>().FindLocalPlayer();
        if (localPlayer == null) return;
        gunHandeler = localPlayer.GetComponent<GunHandeler>();
    }*/

    private void Awake()
    {
        //Starts a recusive coroutine that runs every two seconds until it finds a localplayer
        //StartCoroutine(CheckForPlayer());
        //NetworkManager.Singleton.OnServerStarted += AssignLocalPlayer;
        NetworkManager.Singleton.OnClientConnectedCallback += AssignLocalPlayerForClients;
    }
    private void Update()
    {
        SwapButtonUpdateFunction();
    }

    private void SwapButtonUpdateFunction()
    {
        //checks whether gunhandeler is null and then checks gunswapossibble then turn on swap button
        if (gunHandeler == null)
        {
            if (localPlayer != null)
            {
                gunHandeler = localPlayer.GetComponent<GunHandeler>();
            }
            return;
        }
        if (gunHandeler.GunSwapPossible)
        {
            SwapButtonSwitch(true);
        }
        else
        {
            SwapButtonSwitch(false);
        }
    }

    IEnumerator CheckForPlayer()
    {
        //recursive coroutine that runs every 2s until it finds the localplayer
        localPlayer = FindObjectOfType<MyNetworkManager>().FindLocalPlayer();
        if(localPlayer != null)
        {
            gunHandeler = localPlayer.GetComponent<GunHandeler>();
        }
        yield return new WaitForSecondsRealtime(2f);
        if(localPlayer == null)
        {
            StartCoroutine(CheckForPlayer());
        }
    }
    private void SwapButtonSwitch(bool value)
    {
        swapButton.SetActive(value);
    }
    public void GunSwapButton()
    {
        /*if(localPlayer == null)
        {
            AssignLocalPlayer();
        }
        if(gunHandeler == null)
        {
            AssignLocalPlayer();
            Debug.Log("LocalPlayer is no more");
        }*/

        //Calls the gunswap function if a gun is within range as this function is called from ui need to make sure no null values are here
        //but the ui button that calls it become active only when there are no null values so we don't need check it here
        if (gunHandeler.GunSwapPossible)
        {
            gunHandeler.GunSwapServerRpc();
        }
        
    }

    //unused methods

    private void AssignLocalPlayer()
    {
        localPlayer = FindObjectOfType<MyNetworkManager>().FindLocalPlayer();
    }

    private void AssignLocalPlayerForClients(ulong a)
    {
        localPlayer = FindObjectOfType<MyNetworkManager>().FindLocalPlayer();
    }

}
