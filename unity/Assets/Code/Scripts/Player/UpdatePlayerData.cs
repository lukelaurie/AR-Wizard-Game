using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UpdatePlayerData : MonoBehaviour
{

    public static UpdatePlayerData Instance { get; private set; }

    [SerializeField] private TMPro.TMP_Text mainMenuCoinText;
    [SerializeField] private TMPro.TMP_Text shopCoinText;

    private PlayerData playerData;
    private List<Action<Dictionary<string, int>>> spellUpdateCallBacks;

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        playerData = GameObject.FindWithTag("GameInfo").GetComponent<PlayerData>();
        spellUpdateCallBacks = new List<Action<Dictionary<string, int>>>();
        UpdatePlayerCoinTotal();
        UpdatePlayerSpells();
    }

    public async void UpdatePlayerCoinTotal()
    {
        string coinTotal = await AccountManager.Instance.GetUserCoins();
        int coins = Int32.Parse(coinTotal);
        playerData.SetCoinTotal(coins);
        mainMenuCoinText.text = $"Coins:\n{coins}";
        shopCoinText.text = $"Coins:\n{coins}";
    }

    public async void UpdatePlayerSpells()
    {
        var spells = await SpellManager.Instance.GetSpells();
        playerData.SetSpells(spells);
        foreach (var e in spellUpdateCallBacks)
        {
            e.Invoke(spells);
        }
    }

    public void AddSpellsUpdateCallBack(Action<Dictionary<string, int>> e)
    {
        spellUpdateCallBacks.Add(e);
    }
}
