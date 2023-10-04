using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerID : NetworkBehaviour
{
    [SerializeField] private ulong id;

    public ulong ID
    {
        get
        {
            return id;
        }
        set
        {
            id = value;
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        //call the sendclientserverrpc function if it is the owner
        if (IsOwner)
        {
            SendClientIDServerRPC();
        }

    }
    [ServerRpc]
    private void SendClientIDServerRPC(ServerRpcParams serverRpcParams = default)
    {
        AssignIDClientRPC(serverRpcParams.Receive.SenderClientId); //sends the cliendID as parameter to the function
    }

    [ClientRpc]
    private void AssignIDClientRPC(ulong value)
    {
        id = value; //Assigns id value from serverrpc params 
    }
    
}
