using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthUiUpdate : MonoBehaviour
{
    [SerializeField] private GameObject healthBar;
    private PlayerData playerData;
    private int PLAYER_HEALTH;

    void Awake()
    {
        playerData = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<PlayerData>();
        PLAYER_HEALTH = 100;
    }

    public void UpdateHealthBars(string roomsJson)
    {
        var partyHealth = GetPlayerInformation(roomsJson);

        // first destroy the old health bars to rerender them
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var partyMember in partyHealth)
        {
            InitHealthBar(partyMember);
        }
    }

    private void InitHealthBar(System.Tuple<string, float> partyMember)
    {
        // Debug.Log("now here:   " + partyMember.Item2);
        GameObject playerHealthBar = Instantiate(healthBar, transform);
        PlayerHealthBar healthBarScript = playerHealthBar.GetComponent<PlayerHealthBar>();

        healthBarScript.UpdateHealthBar(partyMember.Item2, PLAYER_HEALTH);
        healthBarScript.SetPlayerName(partyMember.Item1);
    }

    private List<Tuple<string, float>> GetPlayerInformation(string roomsJson)
    {
        var curRooms = JsonConvert.DeserializeObject<Dictionary<string, float>>(roomsJson);
        var playerHealth = new List<Tuple<string, float>>();
        var sortedPlayerNames = curRooms.Keys.OrderBy(key => key);

        // add the current player information first 
        playerHealth.Add(new Tuple<string, float>(playerData.GetUsername(), playerData.GetHealth()));

        foreach (var key in sortedPlayerNames)
        {
            if (key != playerData.GetUsername())
                playerHealth.Add(new Tuple<string, float>(key, curRooms[key]));
        }

        return playerHealth;
    }
}
