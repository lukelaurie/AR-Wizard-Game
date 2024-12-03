using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Niantic.Protobuf.Collections;
using UnityEngine;

public class RoomHealth : MonoBehaviour
{
    private Dictionary<string, float> roomHealth;

    void Start()
    {
        roomHealth = new Dictionary<string, float>();
    }

    public void UpdatePlayerHealth(string playerName, float newHealth)
    {
        roomHealth[playerName] = newHealth;
    }

    public string SerializeRooms()
    {
        return JsonConvert.SerializeObject(roomHealth);
    }

    public void ResetRoom()
    {
        roomHealth = new Dictionary<string, float>();
    }
}
