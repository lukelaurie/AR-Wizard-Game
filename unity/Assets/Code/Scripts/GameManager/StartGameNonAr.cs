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

                // StartCoroutine(CheckForConnectionTimeout());
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

            // StartCoroutine(CheckForConnectionTimeout());
        });
    }

    private IEnumerator CheckForConnectionTimeout()
    {
        float timout = 2f;
        float timer = 0f;

        while (timer < timout)
        {
            if (NetworkManager.Singleton.IsConnectedClient)
            {
                yield break; // user connected so stop the coroutine
            }

            timer += 1;
            System.Threading.Thread.Sleep(1000);
            yield return null;
        }

        if (!NetworkManager.Singleton.IsConnectedClient)
        {
            Debug.LogError("Connection timed out. Failed to connect to server");
            NetworkManager.Singleton.Shutdown();
        }
    }
}
