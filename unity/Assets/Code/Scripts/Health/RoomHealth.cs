using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Niantic.Protobuf.Collections;
using UnityEngine;

public class RoomHealth : MonoBehaviour
{
    private PlayerData playerData;
    private Dictionary<string, int> roomHealth;

    // Start is called before the first frame update
    void Start()
    {
        roomHealth = new Dictionary<string, int>();
        playerData = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<PlayerData>();
    }

    public void UpdatePlayerHealth(string playerName, int newHealth)
    {
        roomHealth[playerName] = newHealth;
    }

    public string SerializeRooms()
    {
        return JsonConvert.SerializeObject(roomHealth);
    }

    public void ResetRoom()
    {
        roomHealth = new Dictionary<string, int>();
    }
}
