 using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class StartGameNonAr : NetworkBehaviour
{
    [SerializeField] private Button startHost;
    [SerializeField] private Button startClient;


    void Start()
    {
        startHost.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
        });

        startClient.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
        });
    }

}
