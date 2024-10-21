using System;
using System.Collections;
using System.Collections.Generic;
using Niantic.Lightship.SharedAR.Colocalization;
using Niantic.Lightship.SharedAR.Netcode;
using Niantic.Lightship.SharedAR.Rooms;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class StartGameAr : NetworkBehaviour
{
    [SerializeField] private SharedSpaceManager sharedSpaceManager;
    const int MAX_AMOUNT_CLIENTS_ROOM = 5;

    [SerializeField] private Texture2D targetImage;
    [SerializeField] private float targetImageSize;
    private readonly string roomName = "TestRoom";

    [SerializeField] private Button startGameButton;
    [SerializeField] private Button joinServerButton;
    [SerializeField] private Button startServerButton;

    public static event Action OnStartSharedSpaceServer;
    public static event Action OnJoinSharedSpaceClient;
    public static event Action OnStartGame;
    public static event Action OnStartSharedSpace;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        sharedSpaceManager.sharedSpaceManagerStateChanged += SharedSpaceManagerStateChange;

        //startGameButton.onClick.AddListener(StartGame);

        joinServerButton.onClick.AddListener(() =>
        {
            //EC2 INSTANCE: "18.117.251.172"
            JoinServer("127.0.0.1", 7777);
        });

        startServerButton.onClick.AddListener(() =>
        {
            StartServer();
        });

        startGameButton.interactable = false;
    }

    private void SharedSpaceManagerStateChange(SharedSpaceManager.SharedSpaceManagerStateChangeEventArgs obj)
    {
        if (obj.Tracking)
        {
            startGameButton.interactable = true;
            joinServerButton.interactable = false;
            startServerButton.interactable = false;
        }
    }

    void StartSharedSpace()
    {
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

    void JoinServer(string ip, ushort port)
    {
        StartSharedSpace();
        NetworkManager.Singleton.StartClient();
        OnJoinSharedSpaceClient?.Invoke();
        Debug.Log("Starting Client...");
    }

    void StartServer()
    {
        StartSharedSpace();
        NetworkManager.Singleton.StartHost();
        OnStartSharedSpaceServer?.Invoke();
        Debug.Log("Starting The Dedicated Server...");
    }
}