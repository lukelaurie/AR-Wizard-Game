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

        if (Input.GetKeyUp(KeyCode.K))
        {
            var clientData = GameObject.FindWithTag("GameInfo").GetComponent<PlayerData>();
            clientData.PlayerTakeDamage(25);
        }
        if (Input.GetKeyUp(KeyCode.H))
        {
            var clientData = GameObject.FindWithTag("GameInfo").GetComponent<PlayerData>();
            clientData.HealPlayer(25);
        }
    }
}
