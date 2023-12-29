using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Health : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<float> health = new NetworkVariable<float>(default,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private const int maxHealth = 100;
    private PlayerID playerID;

    public override void OnNetworkSpawn()
    {
        health.OnValueChanged += HealthChanged; //Subscribing to the health variable's onvaluechanged event
        playerID = GetComponent<PlayerID>();
        if(IsOwner) health.Value = maxHealth; //Assigns health value as max (as it is the health at spawn)
    }

    public void UpdateHealth(float value,ulong id)
    {
        //decreases health when it is the owner and the player id doesnot match the bullet id (health will get updated to all clients as
        //the health variable is synced so it is enough to change it only in the owner also only the owner can write to the variable)
        if(IsOwner && playerID.ID != id)
        {
            health.Value = health.Value - value;
        }
        KillPlayer(); //checks whether the player's health is zero
    }

    private void KillPlayer()
    {
        if(health.Value <= 0 && IsOwner)
        {
            Debug.Log("I died "+ playerID);
            DestroyPlayerServerRpc();
        }
    }
    //For testing purpose
    private void HealthChanged(float previousValue , float newValue)
    {
        if(IsOwner)
        Debug.Log(newValue);
    }

    public void RestoreHealth()
    {
        if (IsOwner)
        {
            health.Value = maxHealth;
        }
    }

    [ServerRpc]
    private void DestroyPlayerServerRpc(ServerRpcParams serverRpcParams = default)
    {
        //Destroy(gameObject);
        ulong clientId = playerID.ID;
        //gameObject.GetComponent<NetworkObject>().Despawn(true);
        Debug.Log("spawn is initiated in health");
        FindObjectOfType<PlayerSpawner>().PlayerDespawn(clientId);
        FindObjectOfType<PlayerSpawner>().StartSpawn(clientId);
        /*PlayerSpawner.instance.PlayerDespawn(clientId);
        PlayerSpawner.instance.StartSpawn(clientId);*/
    }
    
}

