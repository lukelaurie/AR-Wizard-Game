using System;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGameEvents : NetworkBehaviour
{

    async void Update()
    {
        if (Input.GetKeyUp(KeyCode.A) && GameObject.FindWithTag("GameInfo").GetComponent<PlayerData>().IsPlayerHost())
        {
            AllClientsInvoker.Instance.InvokePartyLoseGameAllClients();
        }

        if (Input.GetKeyUp(KeyCode.B) && GameObject.FindWithTag("GameInfo").GetComponent<PlayerData>().IsPlayerHost())
        {
            AllClientsInvoker.Instance.InvokePlayerDieAllClients();
        }

        if (Input.GetKeyUp(KeyCode.C) && GameObject.FindWithTag("GameInfo").GetComponent<PlayerData>().IsPlayerHost())
        {

            AllClientsInvoker.Instance.InvokePartyWinGameAllClients();
        }
    }
}
