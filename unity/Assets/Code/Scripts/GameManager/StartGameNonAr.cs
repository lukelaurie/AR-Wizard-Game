using System.Collections;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class StartGameNonAr : NetworkBehaviour
{
    [SerializeField] private Button joinServerButton;
    [SerializeField] private Button startServerButton;
    [SerializeField] private Canvas createGameCanvas;
    [SerializeField] private Camera startCamera;

    void Start()
    {
        // check if running on the server 
        if (Application.isBatchMode)
        {
            StartServer();
            return;
        }

        createGameCanvas.gameObject.SetActive(true);

        joinServerButton.onClick.AddListener(() =>
        {
            JoinServer("127.0.0.1", 7777);
        });

        startServerButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });
    }

    void JoinServer(string ip, ushort port)
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ip, port);
        NetworkManager.Singleton.StartClient();
    }

    void StartServer()
    {
        Debug.Log("Starting The Dedicated Host...");
        NetworkManager.Singleton.StartServer();
    }

    public override void OnNetworkSpawn()
    {
        createGameCanvas.gameObject.SetActive(false);
        startCamera.gameObject.SetActive(false);
    }
    
}
