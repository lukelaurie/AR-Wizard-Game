using UnityEngine;
using Unity.Netcode;
using System;

public class AllClientsInvoker : MonoBehaviour
{
    public async void InvokeAllClients(string methodName)
    {
        string[] roomPlayers = await RoomManager.Instance.GetPlayersInRoom();

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            var clientNotifyObj = client.PlayerObject.GetComponent<NotifyClient>();
            var clientData = GameObject.FindWithTag("playerInfo").GetComponent<PlayerData>();

            if (!(clientNotifyObj != null && Array.Exists(roomPlayers, player => player == clientData.GetUsername())))
            {
                continue;
            }
            switch (methodName)
            {
                case "PartyLoseGameClientRpc":
                    clientNotifyObj.PartyLoseGameClientRpc();
                    break;
                case "JoinGameClientRpc":
                    clientNotifyObj.JoinGameClientRpc();
                    break;
                case "PartyWinGameClientRpc":
                    clientNotifyObj.PartyWinGameClientRpc();
                    break;
                case "PlayerDieClientRpc":
                    clientNotifyObj.PlayerDieClientRpc();
                    break;
            }
        }
    }
}
