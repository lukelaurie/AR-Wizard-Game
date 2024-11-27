using UnityEngine;
using Unity.Netcode;
using System;

public class AllClientsInvoker : MonoBehaviour
{
    public async void InvokePartyLoseGameAllClients()
    {
        string[] roomPlayers = await RoomManager.Instance.GetPlayersInRoom();
        await RoomManager.Instance.EndGame("hydra", false, 5); // have the player lose the game

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            var clientNotifyObj = client.PlayerObject.GetComponent<NotifyClient>();
            var clientData = GameObject.FindWithTag("GameInfo").GetComponent<PlayerData>();

            clientNotifyObj.PartyLoseGameClientRpc();
        }
    }
    public async void InvokeJoinGameAllClients()
    {
        string[] roomPlayers = await RoomManager.Instance.GetPlayersInRoom();

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            var clientNotifyObj = client.PlayerObject.GetComponent<NotifyClient>();
            var clientData = GameObject.FindWithTag("GameInfo").GetComponent<PlayerData>();

            clientNotifyObj.JoinGameClientRpc();
        }
    }
    public async void InvokePartyWinGameAllClients()
    {
        BossData bossData = GameObject.FindWithTag("GameInfo").GetComponent<BossData>();
        string[] roomPlayers = await RoomManager.Instance.GetPlayersInRoom();
        string bossName = bossData.GetBossName();
        int bossLevel = bossData.GetBossLevel();

        Debug.Log($"Name: {bossName}      Level: {bossLevel}");
        var rewards = await RoomManager.Instance.EndGame(bossName, true, bossLevel);
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
            var clientData = GameObject.FindWithTag("GameInfo").GetComponent<PlayerData>();

            clientNotifyObj.PlayerDieClientRpc();
        }
    }
}
