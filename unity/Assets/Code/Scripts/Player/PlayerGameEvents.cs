using System;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGameEvents : NetworkBehaviour
{
    private AllClientsInvoker invoker;

    void Start()
    {
        var gameLogic = GameObject.FindWithTag("GameLogic");
        invoker = gameLogic.GetComponent<AllClientsInvoker>();
    }

    async void Update()
    {
        if (Input.GetKeyUp(KeyCode.A) && GameObject.FindWithTag("GameInfo").GetComponent<PlayerData>().IsPlayerHost())
        {
            invoker.InvokePartyLoseGameAllClients();
        }

        if (Input.GetKeyUp(KeyCode.B) && GameObject.FindWithTag("GameInfo").GetComponent<PlayerData>().IsPlayerHost())
        {
            invoker.InvokePlayerDieAllClients();
        }

        if (Input.GetKeyUp(KeyCode.C) && GameObject.FindWithTag("GameInfo").GetComponent<PlayerData>().IsPlayerHost())
        {

            invoker.InvokePartyWinGameAllClients();
        }
    }
}
