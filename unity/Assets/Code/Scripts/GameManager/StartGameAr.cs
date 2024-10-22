using System;
using System.Collections;
using System.Collections.Generic;
using Niantic.Lightship.AR.Settings;
using Niantic.Lightship.SharedAR.Colocalization;
using Niantic.Lightship.SharedAR.Netcode;
using Niantic.Lightship.SharedAR.Rooms;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class StartGameAr : MonoBehaviour
{
    [SerializeField] private SharedSpaceManager sharedSpaceManager;
    [SerializeField] private Texture2D targetImage;
    [SerializeField] private float targetImageSize;

    [SerializeField] private Button startGameButton;
    [SerializeField] private Button joinServerButton;
    [SerializeField] private Button startServerButton;

    public static event Action OnStartSharedSpaceServer;
    public static event Action OnJoinSharedSpaceClient;
    public static event Action OnStartGame;
    public static event Action OnStartSharedSpace;

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        DontDestroyOnLoad(gameObject);

        StartSharedSpace();

        System.Random random = new System.Random();
        int randomNumber = random.Next();
        PrivacyData.SetUserId(randomNumber.ToString());

        // check if running on the server 
        if (Application.isBatchMode)
        {
            StartServer();
            return;
        }

        joinServerButton.onClick.AddListener(() =>
        {
            JoinServer();
        });

        startServerButton.onClick.AddListener(() =>
        {
            StartServer();
        });
    }

    void StartSharedSpace()
    {
        string roomName = "Random1Room";
        const int MAX_AMOUNT_CLIENTS_ROOM = 32;

        OnStartSharedSpace?.Invoke();

        if (sharedSpaceManager.GetColocalizationType() == SharedSpaceManager.ColocalizationType.MockColocalization)
        {
            var mockTrackingArgs = ISharedSpaceTrackingOptions.CreateMockTrackingOptions();
            var roomArgs = ISharedSpaceRoomOptions.CreateLightshipRoomOptions(
                roomName, MAX_AMOUNT_CLIENTS_ROOM, "MockColocalizationDemo"
            );

            sharedSpaceManager.StartSharedSpace(mockTrackingArgs, roomArgs);
            return;
        }

        if (sharedSpaceManager.GetColocalizationType() == SharedSpaceManager.ColocalizationType.ImageTrackingColocalization)
        {
            var imageTrackingOptions = ISharedSpaceTrackingOptions.CreateImageTrackingOptions(
                targetImage, targetImageSize
            );

            var roomArgs = ISharedSpaceRoomOptions.CreateLightshipRoomOptions(
                roomName, MAX_AMOUNT_CLIENTS_ROOM, "ImageColocalizationDemo"
            );

            sharedSpaceManager.StartSharedSpace(imageTrackingOptions, roomArgs);
            return;
        }
    }

    void JoinServer()
    {
        NetworkManager.Singleton.StartClient();
        OnJoinSharedSpaceClient?.Invoke();
        Debug.Log("Starting Client...");
    }

    void StartServer()
    {
        NetworkManager.Singleton.StartServer();
        OnStartSharedSpaceServer?.Invoke();
        Debug.Log("Starting The Dedicated Server...");
    }

    private void OnClientConnectedCallback(ulong clientId)
    {
        Debug.Log($"Client connected: {clientId}");
    }
}