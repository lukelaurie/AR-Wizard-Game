using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public event Action<float> OnPlayerTakeDamage;
    public event Action<float> OnPlayerHealed;

    private string username;
    private int coinTotal;
    private bool isPlayerHost;
    private bool isPlayerDead = false;
    private int selectedBossLevel;

    private readonly float MAX_HEALTH = 50f;
    private float health = 50f;
    private Dictionary<string, int> spells;
    private Texture2D targetImage;


    public string GetUsername()
    {
        return username;
    }

    public int GetCoinTotal()
    {
        return coinTotal;
    }

    public Dictionary<string, int> GetSpells()
    {
        return spells;
    }

    public bool IsPlayerHost()
    {
        return isPlayerHost;
    }

    public bool IsPlayerDead()
    {
        return isPlayerDead;
    }

    public float GetHealth()
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

    public Texture2D GetTargetImage()
    {
        return targetImage;
    }

    public bool IsSpellUnlocked(string spellName)
    {
        return spells.ContainsKey(spellName);
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

    public void PlayerTakeDamage(float damageAmt)
    {
        health -= damageAmt;
        OnPlayerTakeDamage?.Invoke(health);
    }

    public void HealPlayer(float healingAmt)
    {
        health = Math.Min(MAX_HEALTH, health + healingAmt);
        OnPlayerHealed?.Invoke(health);
    }

    public void ResetHealth()
    {
        health = 50f;
    }

    public void SetBossLevel(int bossLevel)
    {
        selectedBossLevel = bossLevel;
    }
    public void SetSpells(Dictionary<string, int> newSpells)
    {
        spells = newSpells;
    }

    public void SetTargetImage(Texture2D newImage)
    {
        targetImage = newImage;
    }
}