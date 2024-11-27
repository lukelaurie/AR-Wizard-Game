using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private string username;
    private bool isPlayerHost;

    public string GetUsername()
    {
        return username;
    }

    public bool IsPlayerHost()
    {
        return isPlayerHost;
    }

    public void SetUsername(string newUsername)
    {
        username = newUsername;
    }

    public void SetIsPlayerHost(bool isHost)
    {
        isPlayerHost = isHost;
    }
}