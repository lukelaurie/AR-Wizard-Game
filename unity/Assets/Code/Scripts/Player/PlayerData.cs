using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private string username;
    private int coinTotal;
    private bool isPlayerHost;
    private Dictionary<string, int> spells;


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

    public void SetSpells(Dictionary<string, int> newSpells)
    {
        spells = newSpells;
    }
}