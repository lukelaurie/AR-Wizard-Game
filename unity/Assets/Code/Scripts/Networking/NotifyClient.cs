using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NotifyClient : NetworkBehaviour
{
    [ClientRpc]
    public void JoinGameClientRpc()
    {
        if (!IsOwner)
        {
            return;
        }

        Debug.Log("valid");


        // if (PlayerData.username == targetUsername)
        // {
        //     StartGameAr.StartNewGame();

        //     NetworkManager.Singleton.StartClient();
        //     Debug.Log("Starting AR Client...");

        //     // gameObject.SetActive(false);
        // }
    }
}
