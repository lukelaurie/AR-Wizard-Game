using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NotifyServer : NetworkBehaviour
{
    [SerializeField] private GameObject fireball;
    private int clientDeathCount = 0;

    [ServerRpc(RequireOwnership = false)]
    public void NotifyClientDeathServerRpc(ServerRpcParams rpcParams = default)
    {
        if (!IsServer)
            return;

        clientDeathCount += 1;

        if (clientDeathCount == NetworkManager.Singleton.ConnectedClients.Count)
        {
            AllClientsInvoker.Instance.InvokePartyLoseGameAllClients();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void NotifyClientHealthServerRpc(string username, float health, ServerRpcParams rpcParams = default)
    {
        RoomHealth roomHealthScript = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<RoomHealth>();
        roomHealthScript.UpdatePlayerHealth(username, health);

        AllClientsInvoker.Instance.InvokePlayerHealthChange(roomHealthScript.SerializeRooms());
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnObjectServerRpc(Vector3 spawnPos, Vector3 direction, string casterUsername, string spell, ServerRpcParams rpcParams = default)
    {
        AllClientsInvoker.Instance.InvokePlayerSpellCast(spawnPos, direction, casterUsername, spell);
    }

    public void ResetClientDeaths()
    {
        clientDeathCount = 0;
    }
}
