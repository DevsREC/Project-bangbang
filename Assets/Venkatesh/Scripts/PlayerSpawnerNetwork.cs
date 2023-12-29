using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerSpawnerNetwork : NetworkBehaviour
{

    private GameObject player;
    public void PlayerSwitch(GameObject player)
    {
        this.player = player;
    }
    [ClientRpc]
    public void PlayerSpawnClientRpc()
    {
        DoThisToSpawnPlayer();
        //player.SetActive(true);
    }

    private void DoThisToSpawnPlayer()
    {
        player.GetComponent<BoxCollider2D>().enabled = true;
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<Rigidbody2D>().gravityScale = 1;
        player.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
        player.GetComponent<Health>().RestoreHealth();
    }

    [ClientRpc]
    public void DestroyPlayerClientRpc()
    {
        if(player == null)
        {
            Debug.Log("playe null");
            return;
        }
        player.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
        player.GetComponent<BoxCollider2D>().enabled = false;
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<Rigidbody2D>().gravityScale = 0;
        //gameObject.SetActive(false);
    }
}
