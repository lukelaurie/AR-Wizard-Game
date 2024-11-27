using UnityEngine;
using Unity.Netcode;
using System;

public class AllClientsInvoker : MonoBehaviour
{
    public async void InvokePartyLoseGameAllClients()
    {
        string[] roomPlayers = await RoomManager.Instance.GetPlayersInRoom();

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            var clientNotifyObj = client.PlayerObject.GetComponent<NotifyClient>();
            var clientData = GameObject.FindWithTag("playerInfo").GetComponent<PlayerData>();

            clientNotifyObj.PartyLoseGameClientRpc();
        }
    }
    public async void InvokeJoinGameAllClients()
    {
        string[] roomPlayers = await RoomManager.Instance.GetPlayersInRoom();

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            var clientNotifyObj = client.PlayerObject.GetComponent<NotifyClient>();
            var clientData = GameObject.FindWithTag("playerInfo").GetComponent<PlayerData>();

            clientNotifyObj.JoinGameClientRpc();
        }
    }
    public async void InvokePartyWinGameAllClients()
    {
        string[] roomPlayers = await RoomManager.Instance.GetPlayersInRoom();
        var rewards = await RoomManager.Instance.EndGame("hydra", true, 5);
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            var clientNotifyObj = client.PlayerObject.GetComponent<NotifyClient>();
            clientNotifyObj.PartyWinGameClientRpc(rewards);
        }
    }
    public async void InvokePlayerDieAllClients()
    {
        string[] roomPlayers = await RoomManager.Instance.GetPlayersInRoom();

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            var clientNotifyObj = client.PlayerObject.GetComponent<NotifyClient>();
            var clientData = GameObject.FindWithTag("playerInfo").GetComponent<PlayerData>();

            clientNotifyObj.PlayerDieClientRpc();
        }
    }
}
