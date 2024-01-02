using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponentUpdater : MonoBehaviour
{
    BoxCollider2D boxCollider;
    PlayerMovement playerMovement;
    GameObject playerVisuals;
    SpriteRenderer spriteRenderer;
    PlayerSpawner playerSpawner;
    Health health;
    PlayerID playerId;
    private void Awake()
    {
        playerId = GetComponent<PlayerID>();
        health = GetComponent<Health>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerMovement = GetComponent<PlayerMovement>();
        playerVisuals = gameObject.transform.GetChild(0).gameObject;
        spriteRenderer = playerVisuals.GetComponent<SpriteRenderer>();
        playerSpawner = FindObjectOfType<PlayerSpawner>();
        Subscribe();
    }

    private void Subscribe()
    {
        playerSpawner.onPlayerDespawn += ToDespawnPlayer;
        playerSpawner.onPlayerSpawn += ToSpawnPlayer;
    }

    private void Unsubscribe()
    {

    }

    private void ToSpawnPlayer(Vector3 position , ulong clientId)
    {
        if (clientId != playerId.ID) return;
        gameObject.transform.position = position;
        boxCollider.isTrigger = false;
        gameObject.isStatic = false;
        playerMovement.enabled = true;
        spriteRenderer.enabled = true;
        health.RestoreHealth();

    }

    private void ToDespawnPlayer(ulong clientId)
    {
        if (clientId != playerId.ID) return;
        spriteRenderer.enabled = false;
        boxCollider.isTrigger = true;
        gameObject.isStatic = true;
        playerMovement.enabled = false;
    }
}
