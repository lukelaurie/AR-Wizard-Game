using UnityEngine;
using Unity.Netcode;
using System;
using System.Threading.Tasks;

public class AllClientsInvoker : MonoBehaviour
{
    public async void InvokePartyLoseGameAllClients()
    {
        string[] roomPlayers = await RoomManager.Instance.GetPlayersInRoom();
        // await EndPlayerGames();

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
        string[] roomPlayers = await RoomManager.Instance.GetPlayersInRoom();
        string rewards = await EndPlayerGames();

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

    private async Task<string> EndPlayerGames()
    {
        BossData bossData = GameObject.FindWithTag("GameInfo").GetComponent<BossData>();
        string bossName = bossData.GetBossName();
        int bossLevel = bossData.GetBossLevel();

        Debug.Log($"Name: {bossName}      Level: {bossLevel}");
        return await RoomManager.Instance.EndGame(bossName, true, bossLevel);
    }
}
