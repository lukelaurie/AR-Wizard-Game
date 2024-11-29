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
            DontDestroyOnLoad(gameObject);
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
        DestoyDragon();
    }

    public void InvokePlayerRestartAllClients()
    {
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

    private async Task<string> EndPlayerGames()
    {
        BossData bossData = GameObject.FindWithTag("Dragon").GetComponent<BossData>();
        string bossName = bossData.GetBossName();
        int bossLevel = bossData.GetBossLevel();

        return await RoomManager.Instance.EndGame(bossName, true, bossLevel);
    }

    private void DestoyDragon()
    {
        GameObject dragon = GameObject.FindWithTag("Dragon");
        if (dragon != null)
        {
            Destroy(dragon);
        }
    }
}
