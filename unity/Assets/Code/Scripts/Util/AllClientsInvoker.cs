using UnityEngine;
using Unity.Netcode;
using System;
using System.Threading.Tasks;

public class AllClientsInvoker : MonoBehaviour
{
    public static AllClientsInvoker Instance { get; private set; } // can only set/modify in this class

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InvokePartyLoseGameAllClients()
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            var clientNotifyObj = client.PlayerObject.GetComponent<NotifyClient>();

            clientNotifyObj.PartyLoseGameClientRpc();
        }
    }
    public void InvokeJoinGameAllClients()
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            var clientNotifyObj = client.PlayerObject.GetComponent<NotifyClient>();

            clientNotifyObj.JoinGameClientRpc();
        }
    }
    public async void InvokePartyWinGameAllClients()
    {
        string rewards = await EndPlayerGames();

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            var clientNotifyObj = client.PlayerObject.GetComponent<NotifyClient>();
            clientNotifyObj.PartyWinGameClientRpc(rewards);
        }
    }

    public void InvokePlayerRestartAllClients()
    {
        RoomHealth roomHealthScript = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<RoomHealth>();
        roomHealthScript.ResetRoom();

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            var clientNotifyObj = client.PlayerObject.GetComponent<NotifyClient>();

            clientNotifyObj.PlayerRestartGameClientRpc();
        }
        DestoyDragon();
    }

    public void InvokePlayerBossPlaced()
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            var clientNotifyObj = client.PlayerObject.GetComponent<NotifyClient>();

            clientNotifyObj.PlayerGameStartedClientRpc();
        }
    }

    public void InvokePlayerHealthChange(string roomsJson)
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            var clientNotifyObj = client.PlayerObject.GetComponent<NotifyClient>();
            clientNotifyObj.OtherPlayerHealthChangeClientRpc(roomsJson);
        }
    }

    public void InvokeBossAttackPlayers(string bossAttack)
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            var clientNotifyObj = client.PlayerObject.GetComponent<NotifyClient>();
            clientNotifyObj.BossAttackPlayersClientRpc(bossAttack);
        }
    } 

    public void InvokePlayerSpellCast(Vector3 spawnPos, Vector3 direction, string casterUsername, string spell)
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            var clientNotifyObj = client.PlayerObject.GetComponent<NotifyClient>();
            clientNotifyObj.SpawnOtherPlayerSpellClientRpc(spawnPos, direction, casterUsername, spell);
        }
    } 

    private async Task<string> EndPlayerGames()
    {
        BossData bossData = GameObject.FindWithTag(TagManager.BossParent).GetComponent<BossData>();
        int bossLevel = bossData.GetBossLevel();

        return await RoomManager.Instance.EndGame(true, bossLevel);
    }

    private void DestoyDragon()
    {
        GameObject dragon = GameObject.FindWithTag(TagManager.BossParent);
        if (dragon != null)
        {
            Destroy(dragon);
        }
    }
}
