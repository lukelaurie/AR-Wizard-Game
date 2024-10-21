using System.Collections;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class StartGameNonAr : NetworkBehaviour
{
    [SerializeField] private Button startHost;
    [SerializeField] private Button startClient;
    [SerializeField] private Button clientJoinSever;

    void Start()
    {
        // check if running on the server 
        if (Application.isBatchMode)
        {
            Debug.Log("Starting The Dedicated Server...");
            NetworkManager.Singleton.StartServer();
        }
        else
        {
            clientJoinSever.onClick.AddListener(() =>
            {
                string ip = "18.117.251.172";
                ushort port = 7777;

                NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ip, port);
                NetworkManager.Singleton.StartClient();
            });
        }

        // by default these will run on localhost for testing purposes
        startHost.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });

        startClient.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }
}
