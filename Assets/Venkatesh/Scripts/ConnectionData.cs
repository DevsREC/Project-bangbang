using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ConnectionData : MonoBehaviour
{
    [SerializeField] MyNetworkManager myNetworkManager;

    public void ChangePort(string port)
    {
        myNetworkManager.ChangePort(port);
    }

    public void ChangeIp(string ip)
    {
        myNetworkManager.ChangeIp(ip);
    }
}
