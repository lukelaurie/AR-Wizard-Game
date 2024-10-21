using System.Collections;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class StartGameNonAr : NetworkBehaviour
{
    [SerializeField] private Button joinServerButton;
    [SerializeField] private Button startServerButton;

    void Start()
    {
        // check if running on the server 
        if (Application.isBatchMode)
        {
            StartServer();
            return;
        }

        joinServerButton.onClick.AddListener(() =>
        {
            //EC2 INSTANCE: "18.117.251.172"
            JoinServer("127.0.0.1", 7777);
        });

        startServerButton.onClick.AddListener(() =>
        {
            StartServer();
        });
    }

    void JoinServer(string ip, ushort port)
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ip, port);
        NetworkManager.Singleton.StartClient();
    }

    void StartServer()
    {
        Debug.Log("Starting The Dedicated Server...");
        NetworkManager.Singleton.StartServer();
    }
}
