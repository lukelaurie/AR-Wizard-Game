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
        Debug.Log(clientDeathCount);
        Debug.Log(NetworkManager.Singleton.ConnectedClients.Count);

        if (clientDeathCount == NetworkManager.Singleton.ConnectedClients.Count)
        {
            AllClientsInvoker.Instance.InvokePartyLoseGameAllClients();
        }
    }

    public void ResetClientDeaths()
    {
        clientDeathCount = 0;
    }
}
