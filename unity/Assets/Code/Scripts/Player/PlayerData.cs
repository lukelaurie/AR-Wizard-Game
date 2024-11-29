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
    private bool isPlayerDead = false;
    private int selectedBossLevel;

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

    public bool IsPlayerDead()
    {
        return isPlayerDead;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetBossLevel()
    {
        return selectedBossLevel;
    }

    public int SelectedBossLevel()
    {
        return selectedBossLevel;
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

    public void SetIsPlayerDead(bool isDead)
    {
        isPlayerDead = isDead;
    }

    public void PlayerTakeDamage(int damageAmt)
    {
        health -= damageAmt;
        OnPlayerTakeDamage?.Invoke(health);
    }

    public void HealPlayer(int healingAmt)
    {
        health += healingAmt;
        OnPlayerHealed?.Invoke(health);
    }

    public void SetBossLevel(int bossLevel)
    {
        selectedBossLevel = bossLevel;
    }
}