using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public event Action<int> OnPlayerTakeDamage;
    public event Action<int> OnPlayerHealed;

    private string username;
    private int coinTotal;
    private bool isPlayerHost;
    [SerializeField] private int health;

    public string GetUsername()
    {
        return username;
    }

    public int GetCoinTotal()
    {
        return coinTotal;
    }

    public bool IsPlayerHost()
    {
        return isPlayerHost;
    }

    public int GetHealth()
    {
        return health;
    }

    public void SetUsername(string newUsername)
    {
        username = newUsername;
    }

    public void SetCoinTotal(int newCoinTotal)
    {
        coinTotal = newCoinTotal;
    }

    public void SetIsPlayerHost(bool isHost)
    {
        isPlayerHost = isHost;
    }

    public void PlayerTakeDamage(int damageAmt)
    {
        health -= damageAmt;
        Debug.Log(1);
        OnPlayerTakeDamage?.Invoke(health);
    }

    public void HealPlayer(int healingAmt)
    {
        health += healingAmt;
        Debug.Log(2);

        OnPlayerHealed?.Invoke(health);
    }
}