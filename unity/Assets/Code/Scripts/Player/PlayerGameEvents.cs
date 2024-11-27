using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerGameEvents : NetworkBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.A) && GameObject.FindWithTag("playerInfo").GetComponent<PlayerData>().IsPlayerHost())
        {
            var invoker = GameObject.FindWithTag("GameLogic").GetComponent<AllClientsInvoker>();
            invoker.InvokeAllClients("PartyLoseGameClientRpc");
        }

        if (Input.GetKey(KeyCode.B) && GameObject.FindWithTag("playerInfo").GetComponent<PlayerData>().IsPlayerHost())
        {
            var invoker = GameObject.FindWithTag("GameLogic").GetComponent<AllClientsInvoker>();
            invoker.InvokeAllClients("PlayerDieClientRpc");
        }

        if (Input.GetKey(KeyCode.C) && GameObject.FindWithTag("playerInfo").GetComponent<PlayerData>().IsPlayerHost())
        {
            var invoker = GameObject.FindWithTag("GameLogic").GetComponent<AllClientsInvoker>();
            invoker.InvokeAllClients("PartyWinGameClientRpc");
        }
    }
}
