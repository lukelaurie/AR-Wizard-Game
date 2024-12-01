using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NotifyServer : NetworkBehaviour
{
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
    public void NotifyClientHealthServerRpc(string username, int health, ServerRpcParams rpcParams = default)
    {
        RoomHealth roomHealthScript = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<RoomHealth>();
        roomHealthScript.UpdatePlayerHealth(username, health);

        AllClientsInvoker.Instance.InvokePlayerHealthChange(roomHealthScript.SerializeRooms());
    }

    public void ResetClientDeaths()
    {
        clientDeathCount = 0;
    }
}
