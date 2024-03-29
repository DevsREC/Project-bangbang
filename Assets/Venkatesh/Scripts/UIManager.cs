using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    private GameObject localPlayer;
    private GunHandeler gunHandeler;
    [SerializeField] private GameObject swapButton;
    [SerializeField] private GameObject reloadButton;
    [SerializeField] private TextMeshProUGUI bulletsLeftText;
    [SerializeField] private TextMeshProUGUI totalBulletLeftText;
    [SerializeField] private TextMeshProUGUI timeLeftText;
    private Reload reloadReference;
    private GameTimer gameTimer;
    private KillCounter killCounter;
    [SerializeField] TextMeshProUGUI[] playersNameInTable;
    [SerializeField] TextMeshProUGUI[] playersKillInTable;
    [SerializeField] TextMeshProUGUI[] playersDeathInTable;
    [SerializeField] GameObject scoreBoardPanel;
    [SerializeField] Canvas hudCanvas;
    [SerializeField] TextMeshProUGUI gameEndedText;
    [SerializeField] GameObject pausePanel;
    /*private void AssignLocalPlayer()
    {   //Assigns localplayer and gunhandeler
        localPlayer = FindObjectOfType<MyNetworkManager>().FindLocalPlayer();
        if (localPlayer == null) return;
        gunHandeler = localPlayer.GetComponent<GunHandeler>();
    }*/

    private void Awake()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += AssignLocalPlayerForClients;
    }

    private void Start()
    {
        gameTimer = FindObjectOfType<GameTimer>();
        killCounter = FindObjectOfType<KillCounter>();
        AssignLocalPlayer();
    }
    private void Update()
    {
        SwapButtonUpdateFunction();
        ReloadButtonUpdateFunction();
        UpdateBulletsLeftUI();
        UpdateTimeLeftText();
    }

    public void HudSwitch(bool value)
    {
        hudCanvas.enabled = value;
    }

    public void GameOverTextSwitch(bool value)
    {
        gameEndedText.enabled = value;
    }

    public void ScoreBoardSwitch(bool value)
    {
        scoreBoardPanel.SetActive(value);
    }

    public void ScoreBoardPanelUpdate()
    {
        if (killCounter == null)
        {
            killCounter = FindObjectOfType<KillCounter>();
            ScoreBoardPanelUpdate();
        }
        else
        {
            for (int i = 0; i < killCounter.playersKillDeathTable.Count; i++)
            {
                playersNameInTable[i].text = killCounter.playersKillDeathTable[i][0].ToString();
                playersKillInTable[i].text = killCounter.playersKillDeathTable[i][1].ToString();
                playersDeathInTable[i].text = killCounter.playersKillDeathTable[i][2].ToString();
            }
        }
        
    }

    public void Pause()
    {
        HudSwitch(false);
        pausePanel.SetActive(true);
    }

    public void Resume()
    {
        HudSwitch(true);
        pausePanel.SetActive(false);
    }

    public void ScoreBoardButton()
    {
        ScoreBoardPanelUpdate();
        if (scoreBoardPanel.activeSelf)
        {
            ScoreBoardSwitch(false);
        }
        else
        {
            ScoreBoardSwitch(true);
        }
    }

    private void UpdateTimeLeftText()
    {
        if(gameTimer == null)
        {
            gameTimer = FindObjectOfType<GameTimer>();
        }else if (gameTimer == null)
        {
            UpdateTimeLeftText();
        }
        else
        {
            timeLeftText.text = gameTimer.timeLeft.Value.ToString();
        }
        
    }

    private void ReloadButtonUpdateFunction()
    {
        if (gunHandeler == null) return;
        if(gunHandeler.gunInstance == null)
        {
            reloadButton.SetActive(false);
        }
        else
        {
            reloadButton.SetActive(true);
        }
    }

    private void UpdateBulletsLeftUI()
    {
        reloadReference?.AssignBulletDetails(bulletsLeftText, totalBulletLeftText);
    }
    private void AssignPlayerComponents()
    {
        gunHandeler = localPlayer.GetComponent<GunHandeler>();
        reloadReference = localPlayer.GetComponent<Reload>();
        gunHandeler.Subscribe(reloadButton);
    }
    private void SwapButtonUpdateFunction()
    {
        //checks whether gunhandeler is null and then checks gunswapossibble then turn on swap button
        if (gunHandeler == null)
        {
            if (localPlayer != null)
            {
                AssignPlayerComponents();
            }
            else
            {
                localPlayer = GameObject.Find("NetworkManager").GetComponent<MyNetworkManager>().FindLocalPlayer();
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

    private void AssignLocalPlayerForClients(ulong a)
    {
        localPlayer = FindObjectOfType<MyNetworkManager>().FindLocalPlayer();
    }

    //unused methods

    private void AssignLocalPlayer()
    {
        localPlayer = GameObject.Find("NetworkManager").GetComponent<MyNetworkManager>().FindLocalPlayer();
    }


}
