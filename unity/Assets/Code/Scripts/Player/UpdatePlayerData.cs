using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePlayerData : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text coinText;
    private PlayerData playerData;

    void Start()
    {
        playerData = GameObject.FindWithTag("GameInfo").GetComponent<PlayerData>();

        UpdatePlayerCoinTotal();
    }

    public async void UpdatePlayerCoinTotal()
    {
        string coinTotal = await AccountManager.Instance.GetUserCoins();
        int coins = Int32.Parse(coinTotal);
        playerData.SetCoinTotal(coins);
        coinText.text = $"Coins:\n{coins}";
    }

}
