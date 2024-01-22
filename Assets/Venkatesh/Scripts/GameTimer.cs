using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameTimer : NetworkBehaviour
{
    [SerializeField] float timeLimit = 10f;
    public NetworkVariable<float> timeLeft = new NetworkVariable<float>(default,
    NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private UIManager uiManager;


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsServer)
        {
            timeLeft.Value = timeLimit;
            StartCoroutine(EndGame());
        }

    }

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();       
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSecondsRealtime(1);
        if(timeLeft.Value-- <= 0)
        {
            GameEndedClientRpc();
        }
        else
        {
            StartCoroutine(EndGame());
        }
        
    }

    [ClientRpc]

    private void GameEndedClientRpc()
    {
        uiManager.ScoreBoardPanelUpdate();
        uiManager.HudSwitch(false);
        uiManager.GameOverTextSwitch(true);
        uiManager.ScoreBoardSwitch(true);
        Debug.Log("Game Ended");
    }
}
